from pymodbus.client.sync import ModbusTcpClient as ModbusClient
from time import sleep

"""
Modbus address 317511 -- 317600, (417511 -- 417600)*
1 -- Version of Device
2 -- Family
3 -- Processor
4 -- Module Type
5 -- Status Code
(6--8) -- Ethernet Address
9 -- RAM Size
10 -- Flash Size
11 -- Batt RAM Size
12 -- DIP Settings
13 -- Media Type
(14--15) -- EPF Count (if supported)
16 -- Run Relay State (if supported)
17 -- Batt Low (if supported)
18 -- Model Number
19 -- Ethernet Speed
(20--90) -- Reserved
Modbus Address 317501 -- 317506, (417501 -- 417506)*
1 -- OS Major Version
2 -- OS Minor Version
3 -- OS Build Version
4 -- Booter Major Version
5 -- Booter Minor Version
6 -- Booter Build Version
should be
Booter Version:	4.0.165
OS Version:	4.0.334
"""


class Koyo:
    def __init__(self, ip_address):
        self.ip_address = ip_address
        self._koyo = ModbusClient(ip_address)
        print("Connected:", self._koyo.connect())
        if self._koyo.connect():
            self._getversions()
            self._get_device_info()

    def _getversions(self):
        data = self._koyo.read_input_registers(17500, 6)
        self.os_version = '.'.join(map(str, data.registers[0:3]))
        self.boot_version = '.'.join(map(str, data.registers[3:6]))

    def _get_device_info(self):
        data = self._koyo.read_input_registers(17510, 19)
        self.device_version = data.registers[0]
        self.family = data.registers[1]

    def disconnect(self):
        self._koyo.close()

    def read_register(self, x):
        read = self._koyo.read_holding_registers(x, count = 1, unit = 1)
        return read.registers[0]


import sys
from erabliere_api_proxy import ErabliereApiProxy

if __name__ == '__main__':
    dl_06 = Koyo('192.168.0.155')  # ip address goes here
    print("OS Vesion", dl_06.os_version, "Boot Version", dl_06.boot_version)

    proxy = ErabliereApiProxy("https://erabliereapi.freddycoder.com", "AzureAD")

    for i in range(3990,4100):
        r = dl_06.read_register(i)
        print(i, r)

#        proxy.creer_capteur(1, "DL06 Reg " + str(i), "n/a", True)
        proxy.envoyer_donnee_capteur(i - 3986, r * 10)

    dl_06.disconnect()
