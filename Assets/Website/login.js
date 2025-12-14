// login.js

// Import necessary Firebase SDKs
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-app.js";
import { getAuth, signInWithEmailAndPassword } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-auth.js";
import { getDatabase, ref, get } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-database.js";

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

// Get the form element (Note: We are looking for 'loginForm' now)
const loginForm = document.getElementById('loginForm');

// PRIVATE FUNCTION TO LOAD PROFILE (Equivalent to your C# LoadUserProfile)
function loadUserProfile(uid) {
    const userRef = ref(db, 'users/' + uid); 

    return get(userRef)
        .then((snapshot) => {
            if (snapshot.exists()) {
                const profile = snapshot.val(); 
                
                // Save profile data to browser's session storage (like a global static class in Unity)
                sessionStorage.setItem('userProfile', JSON.stringify(profile));
                
                return profile; 
            } else {
                console.error("User profile data not found in database.");
                throw new Error('profile-not-found'); 
            }
        })
        .catch((error) => {
            console.error("Failed to load user profile:", error);
            throw error; 
        });
}


// The primary function to handle login submission
loginForm.addEventListener('submit', function(events) {
    events.preventDefault(); 

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    if (!email || !password) {
        alert("Please fill in all fields.");
        return;
    }
    
    // 1. SIGN IN (C# SignInWithEmailAndPasswordAsync)
    signInWithEmailAndPassword(auth, email, password)
        .then((userCredential) => {
            // Authentication SUCCESS!
            const uid = userCredential.user.uid;
            console.log("User signed in successfully! UID:", uid);

            // 2. LOAD USER PROFILE (C# LoadUserProfile(user.UserId) call)
            return loadUserProfile(uid); 
        })
        .then((profileData) => {
            // Profile data loaded successfully!
            alert(`👋 Welcome back, ${profileData.username}!`);
            
            // Redirect user to the home page
            window.location.href = 'index.html'; 
        })
        .catch((error) => {
            // Handle errors
            let displayMessage = "Error signing in. Check your email and password.";
            
            if (error.code === 'auth/user-not-found' || error.code === 'auth/wrong-password') {
                displayMessage = "Incorrect email or password.";
            }

            alert(`Login Failed: ${displayMessage}`);
            console.error(error.code, error.message);
        });
});