In this project, we were asked to develop SOMIOD (Service Oriented Middleware for Interoperability and Open Data), a middleware to improve IoT interoperability using RESTful Web Services and open standards. 
The system supports a hierarchical structure (Applications → Containers → Records & Notifications) and implement CRUD + Locate operations (locate operations are kind of a get but it only
returns the name) with XML data stored in a SQL database.

Creating or deleting records triggers notifications, supporting HTTP (POST) and MQTT.

There is also included, an app to test all endpoints working.
