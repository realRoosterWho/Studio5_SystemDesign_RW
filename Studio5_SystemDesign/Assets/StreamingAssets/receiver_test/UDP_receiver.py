import socket
import json
from PIL import Image
import io

def udp_receiver(port):
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.bind(('0.0.0.0', port))
    print(f"Listening on port {port}")

    partial_data = bytearray()
    expected_size = None
    data_type = None

    while True:
        data, addr = sock.recvfrom(8192)
        partial_data.extend(data)

        if len(partial_data) < 4:
            continue

        if data_type is None:
            try:
                header = partial_data[:4].decode('utf-8')
                if header in ('TEXT', 'JSON', 'IMAG'):
                    data_type = header
                    partial_data = partial_data[4:]
                else:
                    partial_data.clear()
                    data_type = None
                    continue
            except UnicodeDecodeError as e:
                print(f"Failed to decode header: {e}")
                partial_data.clear()
                data_type = None
                continue

        if data_type == 'TEXT' or data_type == 'JSON':
            try:
                content = partial_data.decode('utf-8')
                if data_type == 'TEXT':
                    print(f"Received text message: {content}")
                    #存储
                    with open('received_text.txt', 'w') as f:
                        f.write(content)
                elif data_type == 'JSON':
                    data_dict = json.loads(content)
                    print(f"Received JSON data: {data_dict}")
                    #存储
                    with open('received_json.json', 'w') as f:
                        json.dump(data_dict, f, indent=4)
                partial_data.clear()
                data_type = None
            except UnicodeDecodeError as e:
                continue

        elif data_type == 'IMAG':
            if len(data) < 8192:
                try:
                    image = Image.open(io.BytesIO(partial_data))
                    image.save('received_image.png')
                    print("Image saved as received_image.png")
                except Exception as e:
                    print(f"Failed to process image data: {e}")
                partial_data.clear()
                data_type = None

if __name__ == "__main__":
    udp_receiver(5005)