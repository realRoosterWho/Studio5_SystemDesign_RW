import socket
import json
import math
from PIL import Image
import io


class UDPSender:
    def __init__(self, server_ip='127.0.0.1', server_port=5005):
        self.server_ip = server_ip
        self.server_port = server_port
        self.chunk_size = 8192

    def send_message(self, message):
        data = message.encode()
        header = 'TEXT'.encode()
        self._send_data_with_header(header, data)

    def send_json(self, data_dict):
        json_data = json.dumps(data_dict).encode()
        header = 'JSON'.encode()
        self._send_data_with_header(header, json_data)

    def send_image(self, image_path):
        with Image.open(image_path) as img:
            img_byte_arr = io.BytesIO()
            img.save(img_byte_arr, format='PNG')
            image_data = img_byte_arr.getvalue()
        header = 'IMAG'.encode()
        self._send_data_with_header(header, image_data)

    def _send_data_with_header(self, header, data):
        data_to_send = header + data
        total_chunks = math.ceil(len(data_to_send) / self.chunk_size)

        for i in range(total_chunks):
            offset = i * self.chunk_size
            chunk = data_to_send[offset:offset + self.chunk_size]
            self._send_chunk(chunk)

    def _send_chunk(self, chunk):
        udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        udp_socket.sendto(chunk, (self.server_ip, self.server_port))
        udp_socket.close()

    def send_default_image(self, default_image_path):
        self.send_image(default_image_path)

    def send_default_dict(self, default_dict):
        self.send_json(default_dict)


if __name__ == "__main__":
    sender = UDPSender()

    # Example usages
    sender.send_message("Hello from Python!")
    sample_dict = {"key1": "value1", "key2": "value2"}
    sender.send_json(sample_dict)
    sender.send_image("/Volumes/Rooster_SSD/_Unity_Projects/Studio5/Studio5_SystemDesign_RW/Studio5_SystemDesign/Assets/StreamingAssets/receiver_test/path_to_image.png")