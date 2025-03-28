import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../../services/api/user_service.dart';
import '../../services/global/localization.dart';

class ProfilePage extends StatefulWidget {
  final int userID;

  const ProfilePage({super.key, required this.userID});

  @override
  _ProfilePageState createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  final _formKey = GlobalKey<FormState>();
  final TextEditingController _firstNameController = TextEditingController();
  final TextEditingController _lastNameController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _phoneNumController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final TextEditingController _confirmPasswordController = TextEditingController();

  File? _profileImage;
  String? _profileImagePath;
  bool _isLoading = false;
  static const _imagePickerChannel = MethodChannel('com.example.bestworstapp/image_picker');
  late Future<void> _userFuture;

  @override
  void initState() {
    super.initState();
    _userFuture = _initializeProfile();
  }

  Future<void> _initializeProfile() async {
    try {
      final userData = await UserService.getUserByID(widget.userID);
      if (userData['success'] == true) {
        _populateFields(userData['user']);
      } else {
        throw Exception(userData['message'] ?? AppLocalizations.of(context).translate('failed_to_fetch_profile'));
      }
    } catch (e) {
      _showSnackbar('${AppLocalizations.of(context).translate('error_loading_profile')}: $e');
    }
  }

  void _populateFields(Map<String, dynamic> userData) {
    _firstNameController.text = userData['firstName'] ?? '';
    _lastNameController.text = userData['lastName'] ?? '';
    _emailController.text = userData['email'] ?? '';
    _phoneNumController.text = userData['phoneNum'] ?? '';
    _profileImagePath = userData['profileImagePath'];
  }

  Future<void> _pickImage() async {
    try {
      if (Platform.isAndroid) {
        final String? imagePath = await _imagePickerChannel.invokeMethod('pickImage');
        if (imagePath != null) {
          setState(() {
            _profileImage = File(imagePath);
          });
          _showSnackbar(AppLocalizations.of(context).translate('profile_picture_updated'));
        }
      } else {
        _showSnackbar(AppLocalizations.of(context).translate('image_picking_not_supported'));
      }
    } catch (e) {
      _showSnackbar('${AppLocalizations.of(context).translate('error_picking_image')}: $e');
    }
  }

  Future<void> _updateProfile() async {
    if (_formKey.currentState!.validate()) {
      setState(() {
        _isLoading = true;
      });

      try {
        final updatedData = {
          'FirstName': _firstNameController.text.trim(),
          'LastName': _lastNameController.text.trim(),
          'Email': _emailController.text.trim(),
          'PhoneNum': _phoneNumController.text.trim(),
        };

        if (_passwordController.text.isNotEmpty) {
          if (_passwordController.text == _confirmPasswordController.text) {
            updatedData['Password'] = _passwordController.text.trim();
          } else {
            _showSnackbar(AppLocalizations.of(context).translate('passwords_do_not_match'));
            setState(() {
              _isLoading = false;
            });
            return;
          }
        }

        if (_profileImage != null) {
          updatedData['ProfileImagePath'] = _profileImage!.path;
        }

        final result = await UserService.updateUser(widget.userID, updatedData);
        if (result['success'] == true) {
          _showSnackbar(AppLocalizations.of(context).translate('profile_updated_successfully'));
        } else {
          throw Exception(result['message'] ?? AppLocalizations.of(context).translate('failed_to_update_profile'));
        }
      } catch (e) {
        _showSnackbar('${AppLocalizations.of(context).translate('error_updating_user')}: $e');
      } finally {
        setState(() {
          _isLoading = false;
        });
      }
    } else {
      _showSnackbar(AppLocalizations.of(context).translate('please_fill_out_required_fields_correctly'));
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
    _emailController.dispose();
    _phoneNumController.dispose();
    _passwordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final localizations = AppLocalizations.of(context);

    return Scaffold(
        appBar: AppBar(
        title: Text(localizations.translate('edit_my_profile_title')),
    ),
      body: FutureBuilder<void>(
        future: _userFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(
              child: Text(
                snapshot.error.toString(),
                style: const TextStyle(color: Colors.red, fontSize: 16),
              ),
            );
          }
          return Padding(
            padding: const EdgeInsets.all(16.0),
            child: Form(
              key: _formKey,
              child: SingleChildScrollView(
                child: Column(
                  children: [
                    GestureDetector(
                      onTap: _pickImage,
                      child: CircleAvatar(
                        radius: 60,
                        backgroundImage: _profileImage != null
                            ? FileImage(_profileImage!)
                            : _profileImagePath != null
                            ? NetworkImage(_profileImagePath!)
                            : null,
                        child: (_profileImage == null && _profileImagePath == null)
                            ? const Icon(Icons.person, size: 60)
                            : null,
                      ),
                    ),
                    const SizedBox(height: 16),
                    _buildTextField(
                      localizations.translate('first_name'),
                      _firstNameController,
                      validator: (value) {
                        if (value == null || value.trim().isEmpty) {
                          return localizations.translate('first_name_required'); // Localized validation
                        }
                        return null;
                      },
                    ),
                    const SizedBox(height: 16),
                    _buildTextField(
                      localizations.translate('last_name'),
                      _lastNameController,
                      validator: (value) {
                        if (value == null || value.trim().isEmpty) {
                          return localizations.translate('last_name_required'); // Localized validation
                        }
                        return null;
                      },
                    ),
                    const SizedBox(height: 16),
                    _buildTextField(
                      localizations.translate('email'),
                      _emailController,
                      keyboardType: TextInputType.emailAddress,
                      validator: (value) {
                        if (value == null || value.trim().isEmpty) {
                          return localizations.translate('email_required'); // Localized validation
                        }
                        final emailRegex = RegExp(r'^[^@]+@[^@]+\.[^@]+');
                        if (!emailRegex.hasMatch(value.trim())) {
                          return localizations.translate('valid_email_required'); // Localized validation
                        }
                        return null;
                      },
                    ),
                    const SizedBox(height: 16),
                    _buildTextField(
                      localizations.translate('phone_number'),
                      _phoneNumController,
                      keyboardType: TextInputType.number,
                      maxLength: 8,
                      validator: (value) {
                        if (value == null || value.trim().isEmpty) {
                          return localizations.translate('phone_number_required'); // Localized validation
                        }
                        if (value.trim().length != 8 || !RegExp(r'^\d+$').hasMatch(value)) {
                          return localizations.translate('valid_phone_number_required'); // Localized validation
                        }
                        return null;
                      },
                    ),
                    const SizedBox(height: 16),
                    _buildTextField(
                      localizations.translate('new_password'),
                      _passwordController,
                      obscureText: true,
                      hintText: localizations.translate('new_password_hint'), // Localized hint
                      hintStyle: Theme.of(context).brightness == Brightness.dark
                          ? const TextStyle(color: Colors.grey)
                          : const TextStyle(color: Colors.black54),
                      validator: (value) {
                        if (value != null && value.trim().isNotEmpty) {
                          if (value.trim().length < 8) {
                            return localizations.translate('password_length_required'); // Localized validation
                          }
                        }
                        return null;
                      },
                    ),
                    const SizedBox(height: 16),
                    _buildTextField(
                      localizations.translate('confirm_new_password'),
                      _confirmPasswordController,
                      obscureText: true,
                      hintText: localizations.translate('confirm_new_password_hint'), // Localized hint
                      hintStyle: Theme.of(context).brightness == Brightness.dark
                          ? const TextStyle(color: Colors.grey)
                          : const TextStyle(color: Colors.black54),
                      validator: (value) {
                        if (_passwordController.text.isNotEmpty &&
                            value != _passwordController.text) {
                          return localizations.translate('passwords_do_not_match'); // Localized validation
                        }
                        return null;
                      },
                    ),
                    const SizedBox(height: 16),
                    _isLoading
                        ? const CircularProgressIndicator()
                        : ElevatedButton(
                      onPressed: _updateProfile, // Localized button text
                      style: ElevatedButton.styleFrom(
                        minimumSize: const Size(double.infinity, 50), // Full-width button
                      ),
                      child: Text(localizations.translate('save_changes_button')),
                    ),
                  ],
                ),
              ),
            ),
          );
        },
      ),
    );
  }

  Widget _buildTextField(
      String label,
      TextEditingController controller, {
        TextInputType keyboardType = TextInputType.text,
        bool obscureText = false,
        int? maxLength,
        String? hintText,
        TextStyle? hintStyle,
        String? Function(String?)? validator,
      }) {
    final localizations = AppLocalizations.of(context);
    return TextFormField(
      controller: controller,
      keyboardType: keyboardType,
      obscureText: obscureText,
      maxLength: maxLength,
      decoration: InputDecoration(
        labelText: label,
        hintText: hintText,
        hintStyle: hintStyle ?? const TextStyle(color: Colors.grey),
        border: const OutlineInputBorder(),
        counterText: '',
      ),
      validator: validator ??
              (value) {
            if (value == null || value.trim().isEmpty) {
              return localizations.translate('${label.toLowerCase().replaceAll(' ', '_')}_required'); // Dynamically localized validation key
            }
            return null;
          },
    );
  }
}
