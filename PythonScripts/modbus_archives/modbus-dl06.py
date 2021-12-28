from pymodbus.client.sync import ModbusTcpClient

client = ModbusTcpClient('192.168.0.155')

p = 1

while True:
 print("write coil " + str(p))
 client.write_coil(p, True)
 result = client.read_coils(p,1)
 print(result.bits[0])

 continuer = input("Do you want to continue ? (y/n) (y) :")
 if continuer == 'n':
  break
 else:
  p = p + 1

client.close()
