# server

import socket               # Import socket module
import time

s = socket.socket()         # Create a socket object
host = socket.gethostname() # Get local machine name
port = 12345                # Reserve a port for your service.
s.bind(("localhost", port))        # Bind to the port

s.listen(5)                 # Now wait for client connection.
c, addr = s.accept()     # Establish connection with client.
print('Got connection from', addr)
c.send(b'Thank you for connecting')
while True:
    a=input('Typ iets: ')
    a=str(a)
    a=a.encode()
    a=bytes(a)
    c.send(a)
