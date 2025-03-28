import 'package:flutter/material.dart';
import 'package:intl/intl.dart'; // For date formatting
import '../../services/api/borrow_service.dart'; // Import BorrowService for fetching borrowed devices
import 'history_page.dart'; // Import HistoryPage for navigation

class MyDevicesPage extends StatefulWidget {
  final int userID; // UserID passed to fetch user-specific devices

  const MyDevicesPage({super.key, required this.userID});

  @override
  _MyDevicesPageState createState() => _MyDevicesPageState();
}

class _MyDevicesPageState extends State<MyDevicesPage> {
  late Future<List<dynamic>> _deviceListFuture;
  List<dynamic> _filteredDevices = []; // Devices to display after fetching

  @override
  void initState() {
    super.initState();
    _deviceListFuture = _fetchDevices();
  }

  Future<List<dynamic>> _fetchDevices() async {
    try {
      // Fetch borrowed devices for the specific user
      final devices = await BorrowService.getBorrowRecordsByUser(widget.userID);
      setState(() {
        _filteredDevices = devices; // Assign fetched devices
      });
      return devices;
    } catch (e) {
      print('Error fetching devices: $e');
      setState(() {
        _filteredDevices = [];
      });
      return [];
    }
  }

  // Format borrow date to a readable format
  String _formatBorrowDate(String? borrowDate) {
    if (borrowDate == null || borrowDate.isEmpty) return 'Unknown time';
    final dateTime = DateTime.tryParse(borrowDate);
    if (dateTime == null) return 'Invalid date';
    return DateFormat('yyyy-MM-dd HH:mm').format(dateTime);
  }

  void _navigateToHistoryPage() {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => HistoryPage(userID: widget.userID), // Pass userID to HistoryPage
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('My Devices'),
      ),
      body: Column(
        children: [
          // History Button
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: ElevatedButton.icon(
              onPressed: _navigateToHistoryPage,
              icon: const Icon(Icons.history, color: Colors.white),
              label: const Text(
                'History',
                style: TextStyle(color: Colors.white),
              ),
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.blue, // History button color
                padding: const EdgeInsets.symmetric(vertical: 16.0), // Height of button
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(8.0), // Rounded edges
                ),
                minimumSize: const Size.fromHeight(50), // Full-width button
              ),
            ),
          ),
          // Borrowed Devices List
          Expanded(
            child: FutureBuilder<List<dynamic>>(
              future: _deviceListFuture,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(child: CircularProgressIndicator());
                } else if (snapshot.hasError) {
                  return Center(
                    child: Text(
                      'Failed to load devices: ${snapshot.error}',
                      textAlign: TextAlign.center,
                    ),
                  );
                } else if (_filteredDevices.isEmpty) {
                  return const Center(
                    child: Text(
                      'No borrowed devices found.',
                      style: TextStyle(fontSize: 16, color: Colors.grey),
                    ),
                  );
                } else {
                  return ListView.builder(
                    itemCount: _filteredDevices.length,
                    itemBuilder: (context, index) {
                      final device = _filteredDevices[index];
                      return Card(
                        margin: const EdgeInsets.symmetric(
                          vertical: 8.0,
                          horizontal: 16.0,
                        ),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(12.0),
                        ),
                        elevation: 4,
                        child: ListTile(
                          leading: device['imagePath'] != null &&
                              device['imagePath'].isNotEmpty
                              ? ClipRRect(
                            borderRadius: BorderRadius.circular(8.0),
                            child: Image.network(
                              device['imagePath'],
                              width: 60,
                              height: 60,
                              fit: BoxFit.cover,
                              errorBuilder: (context, error, stackTrace) =>
                              const Icon(
                                Icons.devices,
                                size: 60,
                                color: Colors.grey,
                              ),
                            ),
                          )
                              : const Icon(
                            Icons.devices,
                            size: 60,
                            color: Colors.grey,
                          ),
                          title: Text(
                            device['deviceName'] ?? 'Unnamed Device',
                            style: const TextStyle(fontWeight: FontWeight.bold),
                          ),
                          subtitle: Text(
                            'Borrowed on: ${_formatBorrowDate(device['borrowDate'])}',
                          ),
                          trailing: const Icon(
                            Icons.check_circle,
                            color: Colors.green,
                            size: 30,
                          ),
                        ),
                      );
                    },
                  );
                }
              },
            ),
          ),
        ],
      ),
    );
  }
}
