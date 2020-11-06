import csv
import urllib.request
import requests
import random
import json
from datetime import datetime
import uuid
import string

StockSaveUrl= "http://locahost:5001/v1.0/actors/ReservationItemActor/{id}/method/AddReservation"
StockGetUrl= "http://localhost:5001/v1.0/actors/ReservationItemActor/{id}/method/GetBalance"

for i in range(0,1000):
    sku = 'cookie' + str(i).zfill(3)
    data = -10000

    url = StockSaveUrl.format(id=sku)

    response = requests.post(url, json=data)
            
    if (response.status_code != 200):
        print(f'\tSKU:{sku}, error status code: {response.status_code}')
    else:
        url = StockGetUrl.format(id=sku)
        response = requests.get(url)

        if (response.status_code != 200):
            print(f'\tSKU:{sku}, error status code: {response.status_code}')
        else:
            print(f'\tSKU:{sku}, Balance: {response.text}')   



