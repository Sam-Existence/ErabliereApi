:: Scan a network

for /L %%x in (1,1,254) do ping -w 30 -n 1 192.168.1.%%x | find "TTL" >> ping-scan.txt

arp -a

type ping-scan.txt

del ping-scan.txt

:: Physical address may be 00-e0-62-21-7a-58
