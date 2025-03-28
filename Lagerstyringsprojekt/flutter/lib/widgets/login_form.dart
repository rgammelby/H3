import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../../services/global/localization.dart';

class LoginForm extends StatefulWidget {
  final TextEditingController emailController;
  final TextEditingController passwordController;
  final GlobalKey<FormState> formKey;

  const LoginForm({
    super.key,
    required this.emailController,
    required this.passwordController,
    required this.formKey,
  });

  @override
  _LoginFormState createState() => _LoginFormState();
}

class _LoginFormState extends State<LoginForm> {
  bool _isPasswordVisible = false;

  @override
  Widget build(BuildContext context) {
    final localizations = AppLocalizations.of(context);

    // Debug logs for context and localization keys
    //debugPrint("Localization test in LoginForm: email_label -> ${localizations.translate('email_label')}");
    //debugPrint("Localization test in LoginForm: password_label -> ${localizations.translate('password_label')}");

    return Form(
      key: widget.formKey,
      child: Column(
        children: [
          // Email Field
          TextFormField(
            controller: widget.emailController,
            decoration: InputDecoration(
              labelText: localizations.translate('email_label'),
              prefixIcon: const Icon(Icons.email),
            ),
            keyboardType: TextInputType.emailAddress,
            inputFormatters: [
              FilteringTextInputFormatter.deny(RegExp(r'\s')), // Prevent spaces
            ],
            validator: (value) {
              if (value == null || value.trim().isEmpty) {
                return localizations.translate('enter_email_error');
              }
              if (value.contains(' ')) {
                return localizations.translate('email_no_spaces');
              }
              if (!RegExp(r'^[^@]+@[^@]+\.[^@]+').hasMatch(value.trim())) {
                return localizations.translate('email_invalid');
              }
              return null; // Valid input
            },
          ),
          const SizedBox(height: 16),

          // Password Field
          TextFormField(
            controller: widget.passwordController,
            decoration: InputDecoration(
              labelText: localizations.translate('password_label'),
              prefixIcon: const Icon(Icons.lock),
              suffixIcon: IconButton(
                icon: Icon(
                  _isPasswordVisible ? Icons.visibility : Icons.visibility_off,
                ),
                onPressed: () {
                  setState(() {
                    _isPasswordVisible = !_isPasswordVisible; // Toggle visibility
                  });
                },
              ),
            ),
            obscureText: !_isPasswordVisible, // Password visibility toggle
            validator: (value) {
              if (value == null || value.trim().isEmpty) {
                return localizations.translate('enter_password_error'); // Localized error
              }
              return null; // Valid input
            },
          ),
        ],
      ),
    );
  }
}
