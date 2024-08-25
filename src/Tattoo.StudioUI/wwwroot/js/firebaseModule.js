//// firebaseModule.js
//import { initializeApp } from 'https://www.gstatic.com/firebasejs/9.0.0/firebase-app.js';
//import { getStorage, ref, getDownloadURL } from 'https://www.gstatic.com/firebasejs/9.0.0/firebase-storage.js';

//const firebaseConfig = {
//    // Tu configuración de Firebase
//};

//const app = initializeApp(firebaseConfig);
//const storage = getStorage(app);

//export async function testFirebaseConnection() {
//    try {
//        if (app && storage) {
//            return 'Conexión exitosa a Firebase';
//        } else {
//            return 'Conexión fallida a Firebase: Instancias de app o storage no definidas';
//        }
//    } catch (error) {
//        return 'Conexión fallida a Firebase: ' + error;
//    }
//}

//export function getFirebaseImageUrl(storagePath) {
//    const storageRef = ref(storage, storagePath);
//    return getDownloadURL(storageRef)
//        .then((url) => {
//            return url;
//        })
//        .catch((error) => {
//            console.error("Error getting Firebase image URL:", error);
//            return '';
//        });
//}
