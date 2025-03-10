import { StyleSheet } from 'react-native';
import { Image, type ImageSource } from "expo-image";

type Props = {
    /* imgSource: string; 
    change from string to ImageSource to handle varisous image sources type
    ImageSource is a type from expo-image that can handle both local files (require(...)) and online images (URLs).
    This means both placeholder images and selected images will work.
    */
    imgSource: ImageSource;
    // optional string for user-selected image
    selectedImage? : string;
};

export default function ImageViewer({ imgSource, selectedImage }: Props){
    // wrap selectedImage in { uri: selectedImage } 
    // instead of just passing the string is because 
    // React Native's <Image> component requires an object, not just a string.
    const imageSource = selectedImage ? { uri:selectedImage } : imgSource;

    return <Image source={imageSource} style={styles.image} />
}

const styles = StyleSheet.create({
    image: {
        width: 320,
        height: 440,
        borderRadius: 18,
    },
});
