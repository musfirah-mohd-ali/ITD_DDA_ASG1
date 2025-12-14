// Import necessary Firebase SDKs
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-app.js";
import { getAuth, createUserWithEmailAndPassword } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-auth.js";
import { getDatabase, ref, set } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-database.js";

// Your web app's Firebase configuration
const firebaseConfig = {
    apiKey: "AIzaSyAwrZYN7j9XYuEKoXTOFAodWY_EJGdD3SM",
    authDomain: "itd-dda-asg1-ca74e.firebaseapp.com",
    databaseURL: "https://itd-dda-asg1-ca74e-default-rtdb.asia-southeast1.firebasedatabase.app",
    projectId: "itd-dda-asg1-ca74e",
    storageBucket: "itd-dda-asg1-ca74e.firebasestorage.app",
    messagingSenderId: "799941127640",
    appId: "1:799941127640:web:3699592b2cdf44c50056ad"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Firebase Services
const auth = getAuth(app);
const db = getDatabase(app);

// Get the form element once the script loads
const signupForm = document.getElementById('signupForm');

// Add an event listener to handle form submission
signupForm.addEventListener('submit', function(events) {
    // Prevents the default form submission which reloads the page
    events.preventDefault(); 
    
    // 1. Get user input values at the time of submission
    const username = document.getElementById('username').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirm-password').value; // Corrected ID reference

    // Basic Validation: Check if passwords match
    if (password !== confirmPassword) {
        alert("Error: Passwords do not match!");
        return; // Stop the function
    }

    // 2. Firebase Authentication: Create user
    createUserWithEmailAndPassword(auth, email, password)
        .then((userCredential) => {
            // Authentication SUCCESS!
            const user = userCredential.user;
            const uid = user.uid;
            
            console.log("User successfully created with UID:", uid);

            // 3. Create the user profile object (Equivalent to UserProfile class in C#)
            const userProfile = {
            username: username,
            email: email,
            collections: { // Equivalent to public CollectionData collections = new CollectionData();
                basic: { // Equivalent to public CollectionType basic = new CollectionType();
                    hasCurry: false, // Equivalent to public bool hasCurry = false;
                    hasWing: false,
                    hasFBalls: false,
                    hasSotong: false
                }
            }
        };

            // 4. Firebase Realtime Database: Save profile data
            // The path is 'users/' + uid
            const dbRef = ref(db, 'users/' + uid);
            
            // set() writes the data to the specified location
            return set(dbRef, userProfile);
        })
        .then(() => {
            // Database saving SUCCESS!
            alert(`✅ Welcome, ${username}! Registration successful. Redirecting to home.`);
            
            // Redirect user to the home page (Equivalent to SwitchPanel(HomePanel) in Unity)
            window.location.href = 'index.html'; 
        })
        .catch((error) => {
            // Handle ALL errors (Authentication or Database saving)
            const errorCode = error.code;
            const errorMessage = error.message;

            let displayMessage = "Registration failed. Please check your network and try again.";

            if (errorCode === 'auth/email-already-in-use') {
                displayMessage = "This email is already registered. Try logging in.";
            } else if (errorCode === 'auth/weak-password') {
                displayMessage = "Password is too weak. Must be at least 6 characters.";
            } else if (errorCode === 'auth/invalid-email') {
                displayMessage = "The email address format is invalid.";
            }
            
            alert(`Error: ${displayMessage}`);
            console.error(errorCode, errorMessage);
        });
});