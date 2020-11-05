SELECT *.id FROM c 
WHERE ARRAY_CONTAINS(c["value"]["Order"].Items, {​​​​​"ProductCode": "bussola2"}​​​​​, true)