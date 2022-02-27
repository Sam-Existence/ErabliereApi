# Scan a network

arp -a

for /L %a in (1,1,254) do start ping 192.168.0.%a

# Physical address may be 00-e0-62-21-7a-58
