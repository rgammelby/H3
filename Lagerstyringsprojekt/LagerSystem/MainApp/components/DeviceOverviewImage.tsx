import { ImageStyle } from "expo-image";
import { useState, useEffect } from "react";
import { Image } from "react-native";


type Props = {
    imageUri: string; // Image URL
    style?: ImageStyle; // Allow external styles to be passed in
};

export default function DeviceOverviewImage({ imageUri, style } :Props) {
    const [imageSrc, setImageSrc] = useState({ uri: `${imageUri}?timestamp=${new Date().getTime()}` });
    // modify the image URL to make it look different each time
    // this will force the image to reload from the server each time it's rendered
    useEffect(() => {
        // Update the imageSrc when the imageUri changes
        setImageSrc({ uri: `${imageUri}?timestamp=${new Date().getTime()}` });
    }, [imageUri]);

    return(
        <Image
            source={imageSrc}
            style={[
                {
                    width: 110,
                    height: 110,
                    resizeMode: "contain",
                    marginRight: 10,
                },
                style,
            ]}
            resizeMode="contain"
            onError={() => {
                console.log(`Image failed to load: ${imageUri}, switching to default.`);
                setImageSrc(require("../assets/images/icon.png")); // Fallback image
            }}
        />
    )
}