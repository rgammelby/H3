import 'api_service.dart';

class DeviceService {
  static Future<Map<String, dynamic>> createDevice(Map<String, dynamic> deviceData) async {
    try {
      //print('Creating Device: $deviceData');

      // Ensure all required fields are present
      final requiredFields = ['DeviceName', 'DeviceType', 'Location', 'Cupboard', 'Status', 'Quantity'];
      if (!requiredFields.every((field) => deviceData.containsKey(field))) {
        throw Exception('Missing required fields for creating a device.');
      }

      // Send API request
      final response = await ApiService.post('Device/Create', deviceData);
      if (response['success'] == true) {
        return response;
      } else {
        throw Exception(response['message'] ?? 'Failed to create device.');
      }
    } catch (e) {
      print('Error creating device: $e');
      return {'success': false, 'error': e.toString()};
    }
  }

  // Fetch all devices
  static Future<List<dynamic>> getAllDevices() async {
    try {
      final response = await ApiService.get('Device/GetAll');
      if (response['success'] == true) {
        print('Devices fetched: ${response['devices']}'); // Debugging
        return response['devices'] ?? [];
      } else {
        throw Exception(response['message'] ?? 'Failed to fetch devices.');
      }
    } catch (e) {
      print('Error fetching devices: $e');
      return [];
    }
  }

  // Fetch soft-deleted devices
  static Future<List<dynamic>> getDeletedDevices() async {
    try {
      final response = await ApiService.get('Device/GetDeleted');
      if (response['success'] == true) {
        print('Deleted devices fetched: ${response['devices']}'); // Debugging
        return response['devices'] ?? [];
      } else {
        throw Exception(response['message'] ?? 'Failed to fetch deleted devices.');
      }
    } catch (e) {
      print('Error fetching deleted devices: $e');
      return [];
    }
  }

  // Fetch device by ID
  static Future<Map<String, dynamic>> getDeviceByID(int deviceID) async {
    try {
      final response = await ApiService.get('Device/GetById/$deviceID');
      if (response['success'] == true) {
        return response['device'];
      } else {
        throw Exception(response['message'] ?? 'Device not found.');
      }
    } catch (e) {
      print('Error fetching device by ID: $e');
      return {'success': false, 'error': e.toString()};
    }
  }

  // Soft delete a device
  static Future<Map<String, dynamic>> softDeleteDevice(int deviceID) async {
    try {
      final response = await ApiService.delete('Device/Delete/$deviceID');
      if (response['success'] == true) {
        return {'success': true, 'message': 'Device soft deleted successfully'};
      } else {
        throw Exception(response['message'] ?? 'Failed to soft delete device.');
      }
    } catch (e) {
      print('Error during soft delete: $e');
      return {'success': false, 'error': e.toString()};
    }
  }

  // Hard delete a device
  static Future<Map<String, dynamic>> hardDeleteDevice(int deviceID) async {
    try {
      final response = await ApiService.delete('Device/HardDelete/$deviceID');
      if (response['success'] == true) {
        return {'success': true, 'message': 'Device permanently deleted successfully'};
      } else {
        throw Exception(response['message'] ?? 'Failed to permanently delete device.');
      }
    } catch (e) {
      print('Error during hard delete: $e');
      return {'success': false, 'error': e.toString()};
    }
  }

  // Restore a device
  static Future<Map<String, dynamic>> restoreDevice(int deviceID) async {
    try {
      final response = await ApiService.put('Device/Restore/$deviceID', {});
      if (response['success'] == true) {
        return {'success': true, 'message': 'Device restored successfully'};
      } else {
        throw Exception(response['message'] ?? 'Failed to restore device.');
      }
    } catch (e) {
      print('Error during device restore: $e');
      return {'success': false, 'error': e.toString()};
    }
  }

  // Upload device image
  static Future<Map<String, dynamic>> uploadDeviceImage(int deviceID, String base64Image, String fileName) async {
    try {
      final response = await ApiService.post('Device/UploadImage/$deviceID', {
        'base64Image': base64Image,
        'fileName': fileName,
      });

      if (response['success'] == true) {
        return {'success': true, 'message': 'Device image uploaded successfully', 'imagePath': response['imagePath']};
      } else {
        throw Exception(response['message'] ?? 'Failed to upload device image.');
      }
    } catch (e) {
      print('Error uploading device image: $e');
      return {'success': false, 'error': e.toString()};
    }
  }

  // Update an existing device
  static Future<Map<String, dynamic>> updateDevice(int deviceID, Map<String, dynamic> updatedData) async {
    try {
      final response = await ApiService.put('Device/Update/$deviceID', updatedData);
      if (response['success'] == true) {
        return response;
      } else {
        throw Exception(response['message'] ?? 'Failed to update device.');
      }
    } catch (e) {
      print('Error during device update: $e');
      return {'success': false, 'error': e.toString()};
    }
  }
}
