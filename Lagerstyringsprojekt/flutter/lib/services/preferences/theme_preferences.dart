import 'dart:io';
import 'package:xml/xml.dart' as xml;

class ThemePreferences {
  bool isShakeEnabled = true;
  int themeA = 0;
  int themeB = 1;

  final String filePath = '/data/user/0/com.example.bestworstapp/files/theme_preferences.xml';

  Future<void> load() async {
    final file = File(filePath);

    if (await file.exists()) {
      try {
        final xmlContent = await file.readAsString();
        final document = xml.XmlDocument.parse(xmlContent);

        isShakeEnabled =
            document.getElement('preferences')?.getElement('isShakeEnabled')?.text.toLowerCase() == 'true';
        themeA = int.tryParse(
            document.getElement('preferences')?.getElement('themeA')?.text ??
                '0') ??
            0;
        themeB = int.tryParse(
            document.getElement('preferences')?.getElement('themeB')?.text ??
                '1') ??
            1;
      } catch (e) {
        print('Error loading theme preferences: $e');
      }
    }
  }

  Future<void> save() async {
    final builder = xml.XmlBuilder();
    builder.processing('xml', 'version="1.0" encoding="UTF-8"');
    builder.element('preferences', nest: () {
      builder.element('isShakeEnabled', nest: isShakeEnabled.toString());
      builder.element('themeA', nest: themeA.toString());
      builder.element('themeB', nest: themeB.toString());
    });

    final document = builder.buildDocument();
    final file = File(filePath);

    try {
      await file.writeAsString(document.toXmlString(pretty: true));
      print('Theme preferences saved to: $filePath');
    } catch (e) {
      print('Error saving theme preferences: $e');
    }
  }
}
