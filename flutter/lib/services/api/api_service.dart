import 'dart:convert';
import 'package:http/http.dart' as http;

class ApiService {
  static const String baseUrl = 'https://XXX.XXX.XX.XX:5048/api';

  // Common method for making GET requests
  static Future<Map<String, dynamic>> get(String endpoint, {Map<String, String>? headers}) async {
    final url = Uri.parse('$baseUrl/$endpoint');
    final response = await http.get(url, headers: headers);

    return _handleResponse(response, 'GET', endpoint);
  }

  // Common method for making POST requests
  static Future<Map<String, dynamic>> post(String endpoint, Map<String, dynamic> body, {Map<String, String>? headers}) async {
    final url = Uri.parse('$baseUrl/$endpoint');
    final response = await http.post(
      url,
      headers: headers ?? {'Content-Type': 'application/json'},
      body: jsonEncode(body),
    );

    return _handleResponse(response, 'POST', endpoint);
  }

  // Common method for making PUT requests
  static Future<Map<String, dynamic>> put(String endpoint, Map<String, dynamic> body, {Map<String, String>? headers}) async {
    final url = Uri.parse('$baseUrl/$endpoint');
    final response = await http.put(
      url,
      headers: headers ?? {'Content-Type': 'application/json'},
      body: jsonEncode(body),
    );

    return _handleResponse(response, 'PUT', endpoint);
  }

  // Common method for making DELETE requests
  static Future<Map<String, dynamic>> delete(String endpoint, {Map<String, String>? headers}) async {
    final url = Uri.parse('$baseUrl/$endpoint');
    final response = await http.delete(url, headers: headers);

    return _handleResponse(response, 'DELETE', endpoint);
  }

  // Private helper method to handle responses
  static Map<String, dynamic> _handleResponse(http.Response response, String method, String endpoint) {
    if (response.statusCode >= 200 && response.statusCode < 300) {
      return jsonDecode(response.body);
    } else {
      // Detailed exception handling for failed requests
      throw ApiException(
        method: method,
        endpoint: endpoint,
        statusCode: response.statusCode,
        responseBody: response.body,
      );
    }
  }
}

// Custom exception for API errors
class ApiException implements Exception {
  final String method;
  final String endpoint;
  final int statusCode;
  final String responseBody;

  ApiException({
    required this.method,
    required this.endpoint,
    required this.statusCode,
    required this.responseBody,
  });

  @override
  String toString() {
    return 'API Error [$method $endpoint]: Status Code: $statusCode | Response: $responseBody';
  }
}
