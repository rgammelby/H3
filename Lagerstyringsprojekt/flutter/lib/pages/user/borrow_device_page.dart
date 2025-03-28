import 'package:flutter/material.dart';
import '../../services/api/borrow_service.dart'; // For borrow actions
import '../../services/api/device_service.dart'; // For fetching device data

class BorrowDevicePage extends StatefulWidget {
  final int userID; // Pass the logged-in user's ID

  const BorrowDevicePage({super.key, required this.userID});

  @override
  _BorrowDevicePageState createState() => _BorrowDevicePageState();
}

class _BorrowDevicePageState extends State<BorrowDevicePage> {
  late Future<List<dynamic>> _devicesFuture;

  @override
  void initState() {
    super.initState();
    _devicesFuture = DeviceService.getAllDevices(); // Fetch all devices
  }

  Future<void> _borrowDevice(Map<String, dynamic> device) async {
    if (device['quantity'] > 0 && device['status'] == "Available") {
      final borrowRequest = {
        "userID": widget.userID,
        "deviceID": device['deviceID'],
      };

      try {
        final result = await BorrowService.logBorrowAction(borrowRequest);

        if (result['success']) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('${device['deviceName']} borrowed successfully!')),
          );

          // Refresh device list
          setState(() {
            _devicesFuture = DeviceService.getAllDevices();
          });
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text(result['message'] ?? 'Failed to borrow device')),
          );
        }
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Error: ${e.toString()}')),
        );
      }
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('${device['deviceName']} is not available for borrowing')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Borrow Device'),
      ),
      body: FutureBuilder<List<dynamic>>(
        future: _devicesFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return Center(child: CircularProgressIndicator()); // Show loader
          } else if (snapshot.hasError) {
            return Center(
              child: Text('Error fetching devices: ${snapshot.error}'),
            );
          } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
            return Center(child: Text('No devices found.'));
          }

          final devices = snapshot.data!;
          return ListView.builder(
            itemCount: devices.length,
            itemBuilder: (context, index) {
              final device = devices[index];
              return Card(
                margin: EdgeInsets.symmetric(vertical: 8.0, horizontal: 16.0),
                child: ListTile(
                  leading: device['imagePath'] != null
                      ? Image.network(
                    device['imagePath'],
                    width: 60,
                    height: 60,
                    fit: BoxFit.cover,
                  )
                      : Icon(
                    Icons.devices,
                    size: 60,
                    color: Colors.grey,
                  ),
                  title: Text(
                    device['deviceName'] ?? 'Unknown Device',
                    style: TextStyle(fontWeight: FontWeight.bold),
                  ),
                  subtitle: Text(
                    'Type: ${device['deviceType']}\n'
                        'Location: ${device['location'] ?? 'Unknown'}\n'
                        'Status: ${device['status']}',
                  ),
                  trailing: ElevatedButton(
                    onPressed: () {
                      if (device['quantity'] > 0 && device['status'] == "Available") {
                        _borrowDevice(device);
                      }
                    },
                    style: ElevatedButton.styleFrom(
                      backgroundColor: (device['quantity'] > 0 && device['status'] == "Available")
                          ? Colors.blue
                          : Colors.grey,
                    ),
                    child: Text('Borrow'),
                  ),
                  isThreeLine: true,
                ),
              );
            },
          );
        },
      ),
    );
  }
}
