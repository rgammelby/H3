import 'package:flutter/material.dart';
import '../../widgets/menu_drawer.dart';
import '../../services/api/user_service.dart';
import '../../services/global/localization.dart';
import '../../services/preferences/preferences_service.dart';
import '../user/return_device_page.dart';

class HomePage extends StatefulWidget {
  final String role; // Role passed from the login page (Admin or User)
  final int userID; // User ID passed for tracking the logged-in user
  final bool isDebugMode; // Indicates if debug mode is enabled
  final PreferencesService preferencesService;

  const HomePage({
    super.key,
    required this.role,
    required this.userID,
    required this.isDebugMode,
    required this.preferencesService,
  });

  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  String? _firstName; // Store the user's first name
  bool _isLoading = true; // Manage loading state

  @override
  void initState() {
    super.initState();
    _fetchFirstName(); // Fetch the first name when the page loads
  }

  Future<void> _fetchFirstName() async {
    try {
      final result = await UserService.getUserByID(widget.userID); // API call
      if (result['success'] == true && result['user']?['firstName'] != null) {
        setState(() {
          _firstName = result['user']['firstName']; // Extract user's first name
          _isLoading = false;
        });
      } else {
        setState(() {
          _firstName = AppLocalizations.of(context).translate('userFallback'); // Fallback value for first name
          _isLoading = false;
        });
      }
    } catch (e) {
      setState(() {
        _firstName = AppLocalizations.of(context).translate('userFallback'); // Fallback value on error
        _isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          AppLocalizations.of(context)
              .translate('homeAppBarTitle')
              .replaceAll('{role}', widget.role),
        ),
      ),
      drawer: MenuDrawer(
        role: widget.role,
        userID: widget.userID,
        preferencesService: widget.preferencesService,
      ),
      body: Center(
        child: _isLoading
            ? const CircularProgressIndicator()
            : Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              '${AppLocalizations.of(context).translate('welcomeMessage').replaceAll('{name}', _firstName ?? AppLocalizations.of(context).translate('userFallback'))}\n${AppLocalizations.of(context).translate('motivationalQuote')}',
              textAlign: TextAlign.center,
              style: const TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(height: 20),
            if (widget.isDebugMode)
              ElevatedButton(
                onPressed: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (context) => ReturnDevicePage(
                        userID: widget.userID,
                      ),
                    ),
                  );
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.red,
                ),
                child: Text(
                  AppLocalizations.of(context)
                      .translate('bypassQrButton'),
                  style: const TextStyle(color: Colors.white),
                ),
              ),
          ],
        ),
      ),
    );
  }
}
