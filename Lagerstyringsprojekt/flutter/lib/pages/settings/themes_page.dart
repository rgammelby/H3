import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../../theme/theme_provider.dart';
import '../../services/preferences/preferences_service.dart';
import '../../services/global/toast_service.dart';

class ThemesPage extends StatefulWidget {
  final PreferencesService preferencesService;
  final int userID;

  const ThemesPage({super.key, required this.preferencesService, required this.userID});

  @override
  State<ThemesPage> createState() => _ThemesPageState();
}

class _ThemesPageState extends State<ThemesPage> {
  late bool _isShakeEnabled; // Enabled state for shake-to-change-theme
  late int _themeAIndex; // Primary theme index
  late int _themeBIndex; // Secondary theme index
  late bool _originalShakeEnabled; // Backup for original shake state
  late int _originalThemeAIndex; // Backup for original primary theme
  late int _originalThemeBIndex; // Backup for original secondary theme
  bool _preferencesChanged = false; // Track if preferences have been modified

  @override
  void initState() {
    super.initState();
    _isShakeEnabled = widget.preferencesService.isShakeEnabled;
    _themeAIndex = widget.preferencesService.themeA;
    _themeBIndex = widget.preferencesService.themeB;

    // Save original state for potential reversion
    _originalShakeEnabled = _isShakeEnabled;
    _originalThemeAIndex = _themeAIndex;
    _originalThemeBIndex = _themeBIndex;
  }

  void _revertPreferences() {
    // Revert to original values
    setState(() {
      _isShakeEnabled = _originalShakeEnabled;
      _themeAIndex = _originalThemeAIndex;
      _themeBIndex = _originalThemeBIndex;

      final themeProvider = Provider.of<ThemeProvider>(context, listen: false);
      themeProvider.setPrimaryThemeIndex(_originalThemeAIndex);
      themeProvider.applyTheme(themeProvider.themeA);
      themeProvider.setSecondaryThemeIndex(_originalThemeBIndex);
    });
    ToastService.showCustomToast(
      context,
      'Changes discarded!',
      backgroundColor: Colors.orange,
    );
  }

  Future<void> _savePreferences() async {
    try {
      widget.preferencesService.isShakeEnabled = _isShakeEnabled; // Update PreferencesService
      widget.preferencesService.themeA = _themeAIndex;
      widget.preferencesService.themeB = _themeBIndex;

      // Save to persistent storage
      await widget.preferencesService.savePreferences();

      // Show success toast
      ToastService.showSuccessToast(context, 'Preferences saved successfully!');
      setState(() {
        _preferencesChanged = false; // Reset change tracker
      });
    } catch (e) {
      // Show error toast in case of failure
      ToastService.showErrorToast(context, 'Failed to save preferences!');
    }
  }

  void _markPreferencesChanged() {
    setState(() {
      _preferencesChanged = true;
    });
  }

  @override
  Widget build(BuildContext context) {
    final themeProvider = Provider.of<ThemeProvider>(context);

    return WillPopScope(
      onWillPop: () async {
        if (_preferencesChanged) {
          _revertPreferences(); // Revert unsaved changes
        }
        return true; // Allow navigation to proceed
      },
      child: Scaffold(
        appBar: AppBar(title: const Text('Themes')),
        body: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Center(
                child: ElevatedButton(
                  onPressed: () {
                    themeProvider.switchThemes(); // Switch themes
                    setState(() {
                      _themeAIndex = themeProvider.primaryThemeIndex;
                      _themeBIndex = themeProvider.secondaryThemeIndex;
                    });
                    _markPreferencesChanged();
                  },
                  style: ElevatedButton.styleFrom(minimumSize: const Size(200, 50)),
                  child: const Text('Switch Themes A <--> B'),
                ),
              ),
              const SizedBox(height: 20),
              ListTile(
                title: const Text('Select Primary (Theme A)'),
                trailing: DropdownButton<int>(
                  value: _themeAIndex,
                  items: const [
                    DropdownMenuItem(value: 0, child: Text('Light Theme')),
                    DropdownMenuItem(value: 1, child: Text('Dark Theme')),
                    DropdownMenuItem(value: 2, child: Text('Custom Theme')),
                  ],
                  onChanged: (int? newValue) {
                    if (newValue != null) {
                      setState(() {
                        _themeAIndex = newValue;
                        themeProvider.setPrimaryThemeIndex(newValue);
                        themeProvider.applyTheme(themeProvider.themeA);
                      });
                      _markPreferencesChanged();
                    }
                  },
                ),
              ),
              ListTile(
                title: const Text('Select Secondary (Theme B)'),
                trailing: DropdownButton<int>(
                  value: _themeBIndex,
                  items: const [
                    DropdownMenuItem(value: 0, child: Text('Light Theme')),
                    DropdownMenuItem(value: 1, child: Text('Dark Theme')),
                    DropdownMenuItem(value: 2, child: Text('Custom Theme')),
                  ],
                  onChanged: (int? newValue) {
                    if (newValue != null) {
                      setState(() {
                        _themeBIndex = newValue;
                        themeProvider.setSecondaryThemeIndex(newValue);
                        themeProvider.applyTheme(themeProvider.themeB);
                      });
                      _markPreferencesChanged();
                    }
                  },
                ),
              ),
              const SizedBox(height: 20),
              SwitchListTile(
                title: const Text('Enable shake to change theme'),
                value: _isShakeEnabled,
                onChanged: (bool value) {
                  setState(() {
                    _isShakeEnabled = value;
                  });
                  _markPreferencesChanged();
                },
              ),
              const Spacer(), // Push "Save Preferences" button to the bottom
              ElevatedButton.icon(
                onPressed: _preferencesChanged ? _savePreferences : null,
                icon: const Icon(Icons.save),
                label: const Text('Save Preferences'),
                style: ElevatedButton.styleFrom(
                  minimumSize: const Size(double.infinity, 50), // Wide button style
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
