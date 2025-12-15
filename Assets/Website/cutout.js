// cutout.js
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-app.js";
import { getAuth, signInWithEmailAndPassword } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-auth.js";
import { getDatabase, ref, get } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-database.js";
import { getDatabase, ref, onValue } from "https://www.gstatic.com/firebasejs/10.7.1/firebase-database.js";


// Your web app's Firebase configuration (Make sure this matches your project)
const firebaseConfig = {
    apiKey: "AIzaSyAwrZYN7j9XYuEKoXTOFAodWY_EJGdD3SM",
    authDomain: "itd-dda-asg1-ca74e.firebaseapp.com",
    databaseURL: "https://itd-dda-asg1-ca74e-default-rtdb.asia-southeast1.firebasedatabase.app",
    projectId: "itd-dda-asg1-ca74e",
    storageBucket: "itd-dda-asg1-ca74e.firebasestorage.app",
    messagingSenderId: "799941127640",
    appId: "1:799941127640:web:3699592b2cdf44c50056ad"
};

// Initialize Firebase services
const app = initializeApp(firebaseConfig);
const auth = getAuth(app); 
const db = getDatabase(app);

// Helper to set image source based on boolean
function applyBoolImage(isUnlocked, imgElement, unlockedSrc, lockedSrc) {
  if (!imgElement) return;
  imgElement.src = isUnlocked ? unlockedSrc : lockedSrc;
}

/* Mapping (mirrors enum+prefab relation in unity app) */
const collectibleMap = {
  hasCurry: {
    elementId: "curryImg",
    unlocked: "pictures/curryPuffCutOut.png",
    locked: "pictures/curryPuffLocked.png"
  },
  hasWing: {
    elementId: "wingImg",
    unlocked: "pictures/wingCutOut.png",
    locked: "pictures/wingLocked.png"
  },
  hasFBalls: {
    elementId: "fballsImg",
    unlocked: "pictures/fishballsCutOut.png",
    locked: "pictures/fishballsLocked.png"
  },
  hasSotong: {
    elementId: "sotongImg",
    unlocked: "pictures/sotongCutOut.png",
    locked: "pictures/sotongLocked.png"
  }
};

/* 👤 Load user data after login */
onAuthStateChanged(auth, (user) => {
  if (!user) return;

  const uid = user.uid;
  const collectionRef = ref(db, `users/${uid}/collections/basic`);

  onValue(collectionRef, (snapshot) => {
    if (!snapshot.exists()) return;

    const data = snapshot.val();

    Object.entries(collectibleMap).forEach(([key, cfg]) => {
      applyBoolImage(
        data[key],
        document.getElementById(cfg.elementId),
        cfg.unlocked,
        cfg.locked
      );
    });
  });
});

