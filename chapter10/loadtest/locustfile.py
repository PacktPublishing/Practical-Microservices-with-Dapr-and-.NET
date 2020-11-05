from locust import HttpUser, TaskSet, task, between
import random
import json
from datetime import datetime
import uuid
import string

def RandomSKU():
    return 'cookie' + str(random.randint(0,999)).zfill(3)

def RandomOrder():
    data = {}
    data['CustomerCode'] = ''.join(random.choices(string.ascii_uppercase + string.digits, k=6))
    data['Date'] = datetime.utcnow().isoformat()

    items, customization = [], []
    chaos = random.random() < 0.2

    maxRange = random.randint(1, 9)
    for itemToOrder in range(0, maxRange):
        item = {}
        if chaos == True and itemToOrder == maxRange -1:
            item['ProductCode'] = 'crazycookie'
        else:
            item['ProductCode'] = RandomSKU()
        item['Quantity'] = 2
        items.append(item)

    maxRange = random.randint(1 if chaos == True else 0, maxRange)
    for itemsToCustomize in range(0, maxRange):
        reverse = (len(items) - 1) - itemsToCustomize
        item = items[reverse]
        item['Quantity'] = 1

        specialrequest = {}
        specialrequest['CustomizationId'] = str(uuid.uuid4())
        specialrequest['Scope'] = item
        customization.append(specialrequest)

    data['Items'] = items
    data['SpecialRequests'] = customization

    return data

class APIUser(HttpUser):
    wait_time = between(0.1, 1) # seconds

    @task(50)
    def getbalance(self):
        SKU = RandomSKU()
        with self.client.get("/balance/%s" % SKU, name="balance", catch_response=True) as response:
           if (not(response.status_code == 201 or 200)):
                    response.failure("Error balance: %s" % response.text)

    @task(1)
    def postorder(self):
        http_headers = {'content-type': 'application/json'}
        payload = RandomOrder()
            
        with self.client.post("/order", json=payload, headers=http_headers, name="order", catch_response=True) as response:
            if (not(response.status_code == 201 or 200)):
                response.failure("Error order: %s" % response.text)