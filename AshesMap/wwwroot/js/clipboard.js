export async function getClipboardImage(dotNetHelper) {
    try {
        if (!navigator.clipboard || !navigator.clipboard.read) {
            throw new Error("Zwischenablage-Zugriff wird nicht unterstützt.");
        }

        const clipboardItems = await navigator.clipboard.read();
        for (const item of clipboardItems) {
            for (const type of item.types) {
                if (type.startsWith("image/")) {
                    const blob = await item.getType(type);
                    const reader = new FileReader();

                    reader.onload = function (event) {
                        try {
                            const base64String = event.target.result.split(',')[1];
                            dotNetHelper.invokeMethodAsync('OnImagePasted', base64String);
                        } catch (err) {
                            console.error("Fehler beim Konvertieren des Bildes:", err);
                            dotNetHelper.invokeMethodAsync('OnClipboardError', "Fehler beim Verarbeiten des Bildes.");
                        }
                    };

                    reader.onerror = function () {
                        console.error("Fehler beim Lesen der Datei.");
                        dotNetHelper.invokeMethodAsync('OnClipboardError', "Fehler beim Lesen der Datei.");
                    };

                    reader.readAsDataURL(blob);
                    return;
                }
            }
        }

        throw new Error("Kein Bild in der Zwischenablage gefunden.");
    } catch (error) {
        console.error("Fehler beim Zugriff auf die Zwischenablage:", error);
        dotNetHelper.invokeMethodAsync('OnClipboardError', error.message);
    }
}
