import 'package:flutter/material.dart';
import '../../services/preferences/preferences_service.dart';
import '../../services/global/localization.dart';
import 'themes_page.dart';
import 'about_page.dart';
import 'music_page.dart';

class SettingsPage extends StatelessWidget {
  final PreferencesService preferencesService;
  final int userID;

  const SettingsPage({super.key, required this.preferencesService, required this.userID});

  @override
  Widget build(BuildContext context) {
    final localizations = AppLocalizations.of(context);

    return Scaffold(
      appBar: AppBar(
        title: Text(localizations.translate('settings_page_title')),
      ),
      body: ListView(
        padding: const EdgeInsets.all(16.0),
        children: [
          // Themes Button
          ListTile(
            leading: const Icon(Icons.color_lens, color: Colors.green),
            title: Text(localizations.translate('themes_page_title')), // Localized button text
            trailing: const Icon(Icons.arrow_forward_ios),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => ThemesPage(
                    preferencesService: preferencesService,
                    userID: userID, // Pass the required userID
                  ),
                ),
              );
            },
          ),
          const Divider(),

          // Music Button
          ListTile(
            leading: const Icon(Icons.music_note, color: Colors.blue),
            title: Text(localizations.translate('music_page_title')), // Localized button text
            trailing: const Icon(Icons.arrow_forward_ios),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => MusicPage(
                    preferencesService: preferencesService, // Pass PreferencesService
                  ),
                ),
              );
            },
          ),
          const Divider(),

          // About Button
          ListTile(
            leading: const Icon(Icons.info, color: Colors.purple),
            title: Text(localizations.translate('about_page_title')), // Localized button text
            trailing: const Icon(Icons.arrow_forward_ios),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => const AboutPage(), // No additional parameters needed
                ),
              );
            },
          ),
          const Divider(),
        ],
      ),
    );
  }
}
