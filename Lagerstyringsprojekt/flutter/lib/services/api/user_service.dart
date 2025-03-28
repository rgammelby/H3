import 'api_service.dart';

class UserService {
  // Login user
  static Future<Map<String, dynamic>> loginUser(String email, String password) async {
    try {
      final response = await ApiService.post('Login', {
        'email': email,
        'password': password,
      });

      //print("Login Response: $response");
      return response;
    } catch (e) {
      //print('Error during login: $e');
      return {'error': e.toString()};
    }
  }

  // Fetch user by ID
  static Future<Map<String, dynamic>> getUserByID(int userID) async {
    try {
      final response = await ApiService.get('User/GetById/$userID');
      return response;
    } catch (e) {
      print('Error fetching user by ID: $e');
      return {'error': e.toString()};
    }
  }

  // Create a new user
  static Future<Map<String, dynamic>> createUser(Map<String, dynamic> userData) async {
    try {
      if (!_validateCreateUserFields(userData)) {
        throw Exception('Missing required fields for creating a user.');
      }

      final response = await ApiService.post('User/Create', userData);
      //print("Create User Response: $response");
      return response;
    } catch (e) {
      //print('Error creating user: $e');
      return {'error': e.toString()};
    }
  }

  // Fetch all users
  static Future<List<dynamic>> getAllUsers() async {
    try {
      final response = await ApiService.get('User/GetAll');
      //print("All Users Response: $response");
      return response['users'] ?? []; // Return the list of users
    } catch (e) {
      print('Error fetching all users: $e');
      return []; // Return an empty list in case of error
    }
  }

  // Soft delete a user
  static Future<Map<String, dynamic>> softDeleteUser(int userID) async {
    try {
      final response = await ApiService.delete('User/Delete/$userID');
      print("Soft Delete Response: $response");
      return response;
    } catch (e) {
      print('Error during soft delete: $e');
      return {'error': e.toString()};
    }
  }

  // Hard delete a user
  static Future<Map<String, dynamic>> hardDeleteUser(int userID) async {
    try {
      final response = await ApiService.delete('User/HardDelete/$userID');
      print("Hard Delete Response: $response");
      return response;
    } catch (e) {
      print('Error during hard delete: $e');
      return {'error': e.toString()};
    }
  }

  // Restore a user
  static Future<Map<String, dynamic>> restoreUser(int userID) async {
    try {
      final response = await ApiService.put('User/Restore/$userID', {});
      print("Restore User Response: $response");
      return response;
    } catch (e) {
      print('Error during user restoration: $e');
      return {'error': e.toString()};
    }
  }

  // Upload profile image
  static Future<Map<String, dynamic>> uploadProfileImage(int userID, String base64Image, String fileName) async {
    try {
      final response = await ApiService.post('User/UploadProfileImage/$userID', {
        'base64Image': base64Image,
        'fileName': fileName,
      });
      print("Upload Profile Image Response: $response");
      return response;
    } catch (e) {
      print('Error during profile image upload: $e');
      return {'error': e.toString()};
    }
  }

  // Update user profile
  static Future<Map<String, dynamic>> updateUser(int userID, Map<String, dynamic> updatedData) async {
    try {
      final response = await ApiService.put('User/Update/$userID', updatedData);
      print("Update User Response: $response");
      return response;
    } catch (e) {
      print('Error updating user: $e');
      return {'error': e.toString()};
    }
  }

  // Private helper to validate required fields for user creation
  static bool _validateCreateUserFields(Map<String, dynamic> userData) {
    return userData.keys.toSet().containsAll(['FirstName', 'LastName', 'Email', 'Password']);
  }
}
