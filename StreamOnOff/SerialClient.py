# client

import socket               # Import socket module

s = socket.socket()         # Create a socket object
host = socket.gethostname() # Get local machine name
port = 12345                # Reserve a port for your service.

s.connect(("localhost", port))
# while True:
#     try:
#         print(s.recv(1024))
# s.close()                     # Close the socket when done
# s.connect((host, port))
while True:
    try:
        print(s.recv(1024))
    except ConnectionResetError:
        print("De connectie is verbroken.")
        break
s.close()                     # Close the socket when done
