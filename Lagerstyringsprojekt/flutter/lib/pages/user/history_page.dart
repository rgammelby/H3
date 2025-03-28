import 'package:flutter/material.dart';
import 'package:intl/intl.dart'; // For date formatting
import '../../services/api/borrow_service.dart'; // For BorrowService API interactions

class HistoryPage extends StatefulWidget {
  final int userID; // UserID passed to fetch user-specific returned devices

  const HistoryPage({super.key, required this.userID});

  @override
  _HistoryPageState createState() => _HistoryPageState();
}

class _HistoryPageState extends State<HistoryPage> {
  late Future<List<dynamic>> _historyFuture; // Holds the history fetch result
  List<dynamic> _userHistory = []; // List to store the user history data

  @override
  void initState() {
    super.initState();
    _historyFuture = _fetchUserHistory();
  }

  // Fetch user history data from BorrowService
  Future<List<dynamic>> _fetchUserHistory() async {
    try {
      final history = await BorrowService.getUserHistory(widget.userID);
      print('Fetched History Records: $history'); // Debugging log
      setState(() {
        _userHistory = history; // Update UI with fetched history
      });
      return _userHistory;
    } catch (e) {
      print('Error fetching user history: $e'); // Log any errors
      setState(() {
        _userHistory = []; // Clear the history in case of failure
      });
      return [];
    }
  }

  // Format dates (borrowed or returned) to a readable format
  String _formatDate(String? date) {
    if (date == null || date.isEmpty) return 'Unknown time'; // Handle null or empty dates
    final dateTime = DateTime.tryParse(date);
    if (dateTime == null) return 'Invalid date'; // Handle invalid date strings
    return DateFormat('yyyy-MM-dd HH:mm').format(dateTime); // Format to a readable string
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('History'),
      ),
      body: FutureBuilder<List<dynamic>>(
        future: _historyFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator()); // Show loading spinner
          } else if (snapshot.hasError) {
            return Center(
              child: Text(
                'Failed to load history: ${snapshot.error}', // Show error message
                textAlign: TextAlign.center,
              ),
            );
          } else if (_userHistory.isEmpty) {
            return const Center(
              child: Text(
                'No history records found.', // Show message if no history is found
                style: TextStyle(fontSize: 16, color: Colors.grey),
              ),
            );
          } else {
            return ListView.builder(
              itemCount: _userHistory.length,
              itemBuilder: (context, index) {
                final record = _userHistory[index];
                return Card(
                  margin: const EdgeInsets.symmetric(vertical: 8.0, horizontal: 16.0),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12.0),
                  ),
                  elevation: 4,
                  child: ListTile(
                    leading: const Icon(
                      Icons.history,
                      size: 40,
                      color: Colors.blue,
                    ),
                    title: Text(
                      record['deviceName'] ?? 'Unnamed Device', // Correct key from API
                      style: const TextStyle(fontWeight: FontWeight.bold),
                    ),
                    subtitle: Text(
                      'Type: ${record['deviceType'] ?? 'Unknown Type'}\n'
                          'Borrowed: ${_formatDate(record['borrowDate'])}\n'
                          'Returned: ${_formatDate(record['returnDate'])}\n'
                          'Duration: ${record['borrowDuration']?.toStringAsFixed(2) ?? 'Unknown'} days', // Display duration
                    ),
                    trailing: const Icon(
                      Icons.check_circle,
                      color: Colors.green,
                      size: 30,
                    ), // Visual indicator for "returned" devices
                  ),
                );
              },
            );
          }
        },
      ),
    );
  }
}
