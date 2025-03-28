import 'package:flutter/material.dart';
import 'package:intl/intl.dart'; // Import for date and time formatting
import '../../services/api/borrow_service.dart'; // Import the BorrowService

class ReturnDevicePage extends StatefulWidget {
  final int userID; // User ID of the logged-in user

  const ReturnDevicePage({super.key, required this.userID});

  @override
  _ReturnDevicePageState createState() => _ReturnDevicePageState();
}

class _ReturnDevicePageState extends State<ReturnDevicePage> {
  bool _isLoading = true; // Manage loading state
  List<dynamic> _borrowedDevices = []; // List of borrowed devices
  List<dynamic> _sortedDevices = []; // List of devices after sorting
  String _selectedSort = 'A-Z'; // Default sorting option

  @override
  void initState() {
    super.initState();
    _fetchBorrowedDevices(); // Fetch borrowed devices when page loads
  }

  // Fetch borrowed devices
  Future<void> _fetchBorrowedDevices() async {
    try {
      final devices = await BorrowService.getBorrowRecordsByUser(widget.userID);
      setState(() {
        _borrowedDevices = devices; // Update borrowed devices list
        _sortedDevices = List.from(devices); // Clone the list for sorting
        _isLoading = false; // Stop loading
      });

      print('_borrowedDevices: $_borrowedDevices'); // Debugging
    } catch (e) {
      print('Error fetching borrowed devices: $e');
      setState(() {
        _borrowedDevices = [];
        _sortedDevices = [];
        _isLoading = false;
      });
    }
  }

  // Sorting Logic
  void _sortDevices(String sortOption) {
    setState(() {
      _selectedSort = sortOption;
      if (sortOption == 'A-Z') {
        _sortedDevices.sort((a, b) =>
            (a['deviceName'] ?? '').compareTo(b['deviceName'] ?? '')); // Ascending
      } else if (sortOption == 'Z-A') {
        _sortedDevices.sort((a, b) =>
            (b['deviceName'] ?? '').compareTo(a['deviceName'] ?? '')); // Descending
      } else if (sortOption == 'Newest First') {
        _sortedDevices.sort((a, b) {
          final dateA = DateTime.parse(a['borrowDate']);
          final dateB = DateTime.parse(b['borrowDate']);
          return dateB.compareTo(dateA); // Descending by date
        });
      } else if (sortOption == 'Oldest First') {
        _sortedDevices.sort((a, b) {
          final dateA = DateTime.parse(a['borrowDate']);
          final dateB = DateTime.parse(b['borrowDate']);
          return dateA.compareTo(dateB); // Ascending by date
        });
      }
    });
  }

  // Format borrow date to show both date and time
  String _formatBorrowDate(String? borrowDate) {
    if (borrowDate == null) return 'Unknown date and time';
    final dateTime = DateTime.parse(borrowDate);
    return DateFormat('yyyy-MM-dd HH:mm').format(dateTime); // Full date and time
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Return Device'),
      ),
      body: Column(
        children: [
          // Sorting Dropdown Menu
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: DropdownButton<String>(
              value: _selectedSort,
              items: ['A-Z', 'Z-A', 'Newest First', 'Oldest First']
                  .map((sortOption) => DropdownMenuItem(
                value: sortOption,
                child: Text(sortOption),
              ))
                  .toList(),
              onChanged: (sortOption) {
                if (sortOption != null) {
                  _sortDevices(sortOption);
                }
              },
              isExpanded: true,
            ),
          ),
          Expanded(
            child: _isLoading
                ? Center(child: CircularProgressIndicator()) // Show a loading spinner
                : _sortedDevices.isEmpty
                ? Center(child: Text('You have not borrowed any devices.'))
                : ListView.builder(
              itemCount: _sortedDevices.length,
              itemBuilder: (context, index) {
                final device = _sortedDevices[index];
                return Card(
                  margin: EdgeInsets.all(8.0),
                  child: ListTile(
                    leading: Icon(Icons.devices),
                    title: Text(device['deviceName'] ?? 'Unknown Device'),
                    subtitle: Text(
                      'Borrowed on: ${_formatBorrowDate(device['borrowDate'])}',
                    ),
                    trailing: ElevatedButton(
                      onPressed: () => _returnDevice(device),
                      child: Text('Return'),
                    ),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  // Handle returning a device
  Future<void> _returnDevice(Map<String, dynamic> device) async {
    try {
      print('Device: $device'); // Log the device being returned

      // Ensure borrowID is valid
      if (device['borrowID'] == null) {
        throw Exception('Borrow ID is null. Cannot return device.');
      }

      // API call to return the device
      final result = await BorrowService.returnDevice(widget.userID, device['borrowID']);
      if (result['success']) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('${device['deviceName']} returned successfully!')),
        );

        // Refresh the list of borrowed devices
        _fetchBorrowedDevices();
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text(result['message'] ?? 'Failed to return device')),
        );
      }
    } catch (e) {
      print('Error while returning device: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('An error occurred. Please try again.')),
      );
    }
  }
}
