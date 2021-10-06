function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
    var email = profile.getEmail()
    var id = profile.getId()
    var id_token = googleUser.getAuthResponse().id_token;
    console.log("this is the id token", id_token)
    const approvedUser = ["nol18003@byui.edu", "leonarine@gmail.com", "tnolasco54@gmail.com"]
    // check with bro murdock on how to add a param to the index.html
    // approvedUser.forEach(element => {
    //     if (element == email) {
    //         location.replace(`http://localhost:5500/Google-Sign-in/front_end/home.html`
    //             // location.replace(`http://localhost:5500/Google-Sign-in/front_end/home.html#${id}`
    //         )
    //     }

    // });
}

function signOut() {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut().then(function () {

        // console.log('User signed out.')
        // location.replace(`http://localhost:5500/Google-Sign-in/front_end/index.html`)


    });
}


// approvedUser.forEach(element => {
//     if (element == email) {
//         location.replace("http://localhost:5500/Google-Sign-in/front_end/home.html")
//     }

// });