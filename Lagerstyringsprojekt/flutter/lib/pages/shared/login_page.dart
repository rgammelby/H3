import 'dart:async';
import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import '../../native_audio.dart';
import '../../services/api/user_service.dart';
import '../../services/global/toast_service.dart';
import '../../services/global/localization.dart';
import '../../services/preferences/preferences_service.dart';
import '../shared/home_page.dart';
import '../../widgets/login_form.dart';
import '../../main.dart';

class LoginPage extends StatefulWidget {
  final PreferencesService preferencesService;

  const LoginPage({super.key, required this.preferencesService});

  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  bool _isLoading = false;
  int _tapCount = 0; // Count taps for activating debug mode
  bool _showDebugButtons = false; // Debug mode state
  Timer? _resetTimer; // Timer for resetting tap sequence
  late FToast _fToast;

  late String _selectedLanguage;

  @override
  void initState() {
    super.initState();
    _fToast = FToast();
    _fToast.init(context);

    // Delay the locale update until after the build phase
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _loadInitialLanguage();
    });
  }

  Future<void> _loadInitialLanguage() async {
    _selectedLanguage = widget.preferencesService.selectedLanguage;
    MyApp.setLocale(context, Locale(_selectedLanguage));
  }

  Future<void> _saveSelectedLanguage(String languageCode) async {
    setState(() {
      _selectedLanguage = languageCode; // Update language in state
      MyApp.setLocale(context, Locale(languageCode)); // Update app locale dynamically
    });

    widget.preferencesService.selectedLanguage = languageCode; // Update PreferencesService
    await widget.preferencesService.savePreferences(); // Persist updated preferences
  }

  void _changeLanguage(String languageCode) {
    _saveSelectedLanguage(languageCode);
  }

  Future<void> _handleLogin() async {
    if (_isLoading) return;

    setState(() => _isLoading = true);

    try {
      final localizations = AppLocalizations.of(context);

      final result = await UserService.loginUser(
        _emailController.text.trim(),
        _passwordController.text.trim(),
      );

      setState(() => _isLoading = false);

      if (result['success'] == true && result['role'] != null && result['userID'] != null) {
        try {
          if (widget.preferencesService.playBGM) {
            await NativeAudio.playBGM(); // Play BGM if enabled
          }
        } catch (e) {
          debugPrint('Error playing BGM: $e');
        }

        Navigator.pushReplacement(
          context,
          MaterialPageRoute(
            builder: (context) => HomePage(
              role: result['role'],
              userID: result['userID'],
              isDebugMode: _showDebugButtons,
              preferencesService: widget.preferencesService,
            ),
          ),
        );
      } else {
        ToastService.showErrorToast(
          context,
          result['message'] ?? localizations.translate('login_failed'),
        );
      }
    } catch (e) {
      setState(() => _isLoading = false);
      ToastService.showErrorToast(
        context,
        '${AppLocalizations.of(context).translate('error')}: ${e.toString()}',
      );
    }
  }

  void _handleLogoTap() {
    _resetTimer?.cancel(); // Cancel any ongoing timer

    if (_showDebugButtons) {
      _showToast(AppLocalizations.of(context).translate('already_bypasser'), false);
      return;
    }

    setState(() => _tapCount++);

    if (_tapCount >= 4 && _tapCount < 10) {
      final countdown = 10 - _tapCount;
      _showToast(
        AppLocalizations.of(context)
            .translate('countdown_taps')
            .replaceFirst('{countdown}', countdown.toString()),
        false,
      );
    } else if (_tapCount == 10) {
      _showToast(AppLocalizations.of(context).translate('bypass_mode_activated'), true);
      setState(() {
        _showDebugButtons = true; // Activate debug mode
        _tapCount = 0; // Reset tap count
      });
    }

    _resetTimer = Timer(const Duration(seconds: 3), () {
      setState(() => _tapCount = 0); // Reset taps
    });
  }

  void _showToast(String message, bool isBypass) {
    _fToast.removeCustomToast();

    if (isBypass) {
      try {
        NativeAudio.playSound();
      } catch (e) {
        debugPrint('Error playing sound: $e');
      }
    }

    final toast = Container(
      padding: const EdgeInsets.symmetric(horizontal: 24.0, vertical: 12.0),
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(25.0),
        color: isBypass ? Colors.green : Colors.black,
      ),
      child: Text(
        message,
        style: const TextStyle(color: Colors.white),
      ),
    );

    _fToast.showToast(
      child: toast,
      toastDuration: Duration(seconds: isBypass ? 2 : 1),
      gravity: ToastGravity.BOTTOM,
    );
  }

  @override
  void dispose() {
    _resetTimer?.cancel();
    _emailController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final localizations = AppLocalizations.of(context);

    return Scaffold(
      appBar: AppBar(
        title: Center(
          child: Text(localizations.translate('login')),
        ),
        actions: [
          DropdownButton<String>(
            value: _selectedLanguage,
            icon: const Icon(Icons.language),
            onChanged: (String? newValue) {
              if (newValue != null) {
                _changeLanguage(newValue);
              }
            },
            items: const [
              DropdownMenuItem(value: 'en', child: Text('English')),
              DropdownMenuItem(value: 'ja', child: Text('Japanese')),
              DropdownMenuItem(value: 'da', child: Text('Danish')),
            ],
          ),
        ],
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              GestureDetector(
                onTap: _handleLogoTap,
                child: Image.asset('assets/images/Logo.png', height: 140),
              ),
              const SizedBox(height: 40),
              LoginForm(
                emailController: _emailController,
                passwordController: _passwordController,
                formKey: _formKey,
              ),
              const SizedBox(height: 20),
              _isLoading
                  ? const CircularProgressIndicator()
                  : ElevatedButton(
                onPressed: () {
                  if (_formKey.currentState?.validate() ?? false) {
                    _handleLogin();
                  }
                },
                child: Text(localizations.translate('login')),
              ),
              const SizedBox(height: 20),
              if (_showDebugButtons)
                Column(
                  children: [
                    Text(
                      localizations.translate('debug_buttons'),
                      style: const TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: [
                        ElevatedButton(
                          onPressed: () {
                            Navigator.pushReplacement(
                              context,
                              MaterialPageRoute(
                                builder: (context) => HomePage(
                                  role: 'Admin',
                                  userID: 1,
                                  isDebugMode: true,
                                  preferencesService: widget.preferencesService,
                                ),
                              ),
                            );
                          },
                          child: Text(localizations.translate('bypass_admin')),
                        ),
                        ElevatedButton(
                          onPressed: () {
                            Navigator.pushReplacement(
                              context,
                              MaterialPageRoute(
                                builder: (context) => HomePage(
                                  role: 'User',
                                  userID: 2,
                                  isDebugMode: true,
                                  preferencesService: widget.preferencesService,
                                ),
                              ),
                            );
                          },
                          child: Text(localizations.translate('bypass_user')),
                        ),
                      ],
                    ),
                  ],
                ),
            ],
          ),
        ),
      ),
    );
  }
}
