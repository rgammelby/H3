import 'dart:io';
import 'package:xml/xml.dart' as xml;

class MusicPreferences {
  bool playBGM = true; // Default value
  final String filePath = '/data/user/0/com.example.bestworstapp/files/music_preferences.xml';

  Future<void> load() async {
    final file = File(filePath);

    if (await file.exists()) {
      try {
        final xmlContent = await file.readAsString();
        final document = xml.XmlDocument.parse(xmlContent);
        playBGM = document.getElement('preferences')?.getElement('playBGM')?.text.toLowerCase() == 'true';
      } catch (e) {
        print('Error loading music preferences: $e');
      }
    }
  }

  Future<void> save() async {
    final builder = xml.XmlBuilder();
    builder.processing('xml', 'version="1.0" encoding="UTF-8"');
    builder.element('preferences', nest: () {
      builder.element('playBGM', nest: playBGM.toString());
    });

    final document = builder.buildDocument();
    final file = File(filePath);

    try {
      await file.writeAsString(document.toXmlString(pretty: true));
      print('Music preferences saved to: $filePath');
    } catch (e) {
      print('Error saving music preferences: $e');
    }
  }
}
