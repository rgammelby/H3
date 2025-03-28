import 'api_service.dart';

class BorrowService {
  static Future<Map<String, dynamic>> logBorrowAction(Map<String, dynamic> borrowData) async {
    try {
      final response = await ApiService.post('Borrow/Create', borrowData);
      print('Borrow Action Logged Successfully: $response');
      return response;
    } catch (e) {
      print('Error Logging Borrow Action: $e');
      return {'success': false, 'message': 'Failed to log borrow action', 'error': e.toString()};
    }
  }

  // Fetch borrow records by a specific user
  static Future<List<dynamic>> getBorrowRecordsByUser(int userID) async {
    try {
      final data = await ApiService.get('Borrow/GetByUser/$userID');
      print('API Response for Borrow Records by User: $data'); // Debugging log
      return data['devices'] ?? []; // Correctly access the "devices" key from the API response
    } catch (e) {
      print('Error Fetching Borrow Records for User: $e');
      return []; // Return empty list on error
    }
  }

  // Fetch borrow records by device ID
  static Future<List<dynamic>> getBorrowRecordsByDevice(int deviceID) async {
    try {
      final data = await ApiService.get('Borrow/GetByDevice/$deviceID');
      print('API Response for Borrow Records by Device: $data'); // Debugging log
      return data['devices'] ?? []; // Adjust based on backend key
    } catch (e) {
      print('Error Fetching Borrow Records for Device: $e');
      return []; // Return empty list on error
    }
  }

  // Fetch user-specific borrow history
  static Future<List<dynamic>> getUserHistory(int userID) async {
    try {
      final data = await ApiService.get('History/GetByUser/$userID'); // Calls the History endpoint
      print('API Response for User History: $data');
      return data['history'] ?? []; // Return history records or an empty list
    } catch (e) {
      print('Error Fetching User History: $e');
      return []; // Return empty list on error
    }
  }

  // Return a borrowed device
  static Future<Map<String, dynamic>> returnDevice(int userID, int borrowID) async {
    try {
      final response = await ApiService.post('Borrow/Return', {
        'userID': userID,
        'borrowID': borrowID,
      });
      print('Device Returned Successfully: $response');
      return response;
    } catch (e) {
      print('Error Returning Device: $e');
      return {'success': false, 'message': 'Failed to return device', 'error': e.toString()};
    }
  }

  // Fetch all borrow records (for admins, includes both returned and active ones)
  static Future<List<dynamic>> getAllBorrowRecords() async {
    try {
      final data = await ApiService.get('Borrow/GetAll');
      print('API Response for All Borrow Records: $data'); // Debugging log
      return data['borrowRecords'] ?? []; // Return empty list if no records found
    } catch (e) {
      print('Error Fetching All Borrow Records: $e');
      return [];
    }
  }

  // Fetch overdue borrow records (for admin functionality)
  static Future<List<dynamic>> getOverdueBorrowRecords() async {
    try {
      final data = await ApiService.get('Borrow/GetOverdue');
      print('API Response for Overdue Borrow Records: $data'); // Debugging log
      return data['borrowRecords'] ?? []; // Return empty list if no records found
    } catch (e) {
      print('Error Fetching Overdue Borrow Records: $e');
      return [];
    }
  }
}
