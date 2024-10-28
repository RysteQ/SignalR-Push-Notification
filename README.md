# SignalR Push Notification

<br>

## Description

This repository is a proof of concept for an easy to implement, low traffic push notification technology for desktop and mobile applications using C#. This project has also a SQLite local database to keep track of unsent notifications so that the user won't miss any important notifications if they were offline for a period of time.

<br>

## Possible improvements

Since this repository is a proof of concept, functional yes but still not meant for any serious usage above, this can be improved upon or modified with minimal effort to increase performance. One possible modification is removing the local database functionality to improve performance, the method that use this functionality arr the ```SendPushNotification``` and ```SendNotificationToGroup``` methods of the RESTful API.

<br>

---

Version 1.1.0
