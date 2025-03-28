import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../../services/api/user_service.dart';
import '../../widgets/image_upload_widget.dart';
import '../../services/global/localization.dart';

class CreateUsersPage extends StatefulWidget {
  const CreateUsersPage({super.key});

  @override
  _CreateUsersPageState createState() => _CreateUsersPageState();
}

class _CreateUsersPageState extends State<CreateUsersPage> {
  final TextEditingController _firstNameController = TextEditingController();
  final TextEditingController _lastNameController = TextEditingController();
  final TextEditingController _phoneNumController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  String _role = ''; // Role will be initialized in didChangeDependencies
  bool _isLoading = false; // Loading state for the submit button
  File? _profileImage; // To store the uploaded profile image

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    // Initialize default role based on localization
    _role = AppLocalizations.of(context).translate('role_user');
  }

  Future<void> _createUser() async {
    setState(() {
      _isLoading = true; // Show loading spinner
    });

    try {
      // Input validation
      if (_firstNameController.text.trim().isEmpty ||
          _lastNameController.text.trim().isEmpty ||
          _phoneNumController.text.trim().isEmpty ||
          _emailController.text.trim().isEmpty ||
          _passwordController.text.trim().isEmpty) {
        _showSnackbar(AppLocalizations.of(context).translate('please_fill_out_all_fields'));
        setState(() {
          _isLoading = false;
        });
        return;
      }

      // Validate phone number (must be exactly 8 digits and numeric)
      final phoneNum = _phoneNumController.text.trim();
      if (!RegExp(r'^\d{8}$').hasMatch(phoneNum)) {
        _showSnackbar(AppLocalizations.of(context).translate('phone_number_validation')); // Localized validation message
        setState(() {
          _isLoading = false;
        });
        return;
      }

      // Validate email (no spaces allowed)
      final email = _emailController.text.trim();
      if (email.contains(' ')) {
        _showSnackbar(AppLocalizations.of(context).translate('email_validation')); // Localized validation message
        setState(() {
          _isLoading = false;
        });
        return;
      }

      // Prepare user data
      final userData = {
        "FirstName": _firstNameController.text.trim(),
        "LastName": _lastNameController.text.trim(),
        "PhoneNum": phoneNum,
        "Email": email,
        "Password": _passwordController.text.trim(),
        "Role": _role == AppLocalizations.of(context).translate('role_admin'), // Convert "Admin"/"User" to boolean
        if (_profileImage != null) "ProfileImagePath": _profileImage!.path,
      };

      //print('User Data: $userData'); // Debugging: Log user data before sending to the API

      // Send data to the API
      final result = await UserService.createUser(userData);

      //print('API Response: $result'); // Debugging: Log API response

      setState(() {
        _isLoading = false; // Stop loading spinner
      });

      if (result['success'] == true) {
        _showSnackbar(AppLocalizations.of(context).translate('user_created_success'));

        // Clear the form
        _firstNameController.clear();
        _lastNameController.clear();
        _phoneNumController.clear();
        _emailController.clear();
        _passwordController.clear();
        setState(() {
          _role = AppLocalizations.of(context).translate('role_user');
          _profileImage = null; // Clear the profile image
        });
      } else {
        _showSnackbar(result['message'] ?? AppLocalizations.of(context).translate('failed_to_create_user'));
      }
    } catch (e) {
      setState(() {
        _isLoading = false; // Stop loading spinner
      });

      // Show error
      _showSnackbar('${AppLocalizations.of(context).translate('error_message')}: ${e.toString()}');
    }
  }

  void _showSnackbar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(content: Text(message), duration: const Duration(seconds: 3)),
    );
  }

  @override
  void dispose() {
    _firstNameController.dispose();
    _lastNameController.dispose();
    _phoneNumController.dispose();
    _emailController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final localizations = AppLocalizations.of(context);

    return Scaffold(
      appBar: AppBar(
        title: Text(localizations.translate('create_user_title')),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: SingleChildScrollView(
          child: Column(
            children: [
              TextField(
                controller: _firstNameController,
                decoration: InputDecoration(labelText: localizations.translate('first_name')),
              ),
              TextField(
                controller: _lastNameController,
                decoration: InputDecoration(labelText: localizations.translate('last_name')),
              ),
              TextField(
                controller: _phoneNumController,
                decoration: InputDecoration(labelText: localizations.translate('phone_number')),
                keyboardType: TextInputType.number, // Numeric keyboard
                inputFormatters: [
                  FilteringTextInputFormatter.digitsOnly, // Restrict to digits only
                  LengthLimitingTextInputFormatter(8), // Limit input to 8 digits
                ],
              ),
              TextField(
                controller: _emailController,
                decoration: InputDecoration(labelText: localizations.translate('email')),
                keyboardType: TextInputType.emailAddress,
                inputFormatters: [
                  FilteringTextInputFormatter.deny(RegExp(r'\s')), // Disallow spaces
                ],
              ),
              TextField(
                controller: _passwordController,
                decoration: InputDecoration(labelText: localizations.translate('password')),
                obscureText: true, // Hide password input
              ),
              DropdownButtonFormField<String>(
                value: _role,
                decoration: InputDecoration(labelText: localizations.translate('role')),
                onChanged: (String? newValue) {
                  setState(() {
                    _role = newValue!;
                  });
                },
                items: <String>[
                  localizations.translate('role_user'),
                  localizations.translate('role_admin'),
                ].map<DropdownMenuItem<String>>((String value) {
                  return DropdownMenuItem<String>(
                    value: value,
                    child: Text(value),
                  );
                }).toList(),
              ),
              const SizedBox(height: 20),
              ImageUploadWidget(
                onImageSelected: (File image) {
                  _profileImage = image; // Store the selected image
                },
              ),
              const SizedBox(height: 20),
              _isLoading
                  ? const CircularProgressIndicator()
                  : ElevatedButton(
                onPressed: _createUser, // Localized button label
                style: ElevatedButton.styleFrom(
                  minimumSize: const Size(double.infinity, 50),
                ),
                child: Text(localizations.translate('create_user_button')),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
