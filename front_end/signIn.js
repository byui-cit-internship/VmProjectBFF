function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
    window.location = 'http://127.0.0.1:5500/front_end/button.html'
    var email = profile.getEmail()
    var id = profile.getId()

      } 

function signOut() {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut().then(function () {
        console.log('User signed out.')
    });
}

// I have used Brother's Murdock code to connect the frontend and backend together
// when we get the backend push to github then I will start working in connecting frontend and backend

// let clicks = 0; 
// let usertoken="";
// let email="";
// let signInbtn;

// $(document).ready(function(){
//     signInbtn = document.getElementById('signInbtn')
//     // I used this code from Brother's Murdock website
//     let hash= location.hash;//will include the #
//     let hashparts = hash.split("#");
//     if (hashparts.length < 2) {
//         window.location="/"; //there is no login token on the url, so they must not have logged in yet, we will help redirect them here
//     } else {
//         usertoken = hashparts[1];// the url should look like https://stedi.me/timer.html#4c2286a7-8fdc-47c5-b972-739769554c88
//         validateToken();//check if token is expired, if not display the email, if expired send to login
//     }
// });

// const saveStudentInfo = (studentInfo) => {
//     $.ajax({
//         type: 'POST',
//         url: '', // we need to add the url from the backend
//         data: JSON.stringify(studentInfo), // or JSON.stringify ({name: 'jonas'}),
//         statusCode:{
//             401: () => window.location.href="/",
//         },
//         headers: { "account.session.token": usertoken},
//         contentType: "application/json",
//         dataType: 'json'
//     });

// }

// const getStudentInfo = () => {
//     $.ajax({
//         type: 'GET',
//         url: ''+$('#email').html(), // we need to add the url from the backend 
//         success: function(data) {
//           let studentInfo = JSON.parse(data);
//           document.getElementById('info').innerHTML = studentInfo.info;
//         },
//         headers: { "account.session.token": usertoken},
//         contentType: "application/json",
//         dataType: 'json'
//     });

//     const validateToken = () => {
//         let tokenEmail="";
//         $.ajax({
//            type: 'GET',
//             url: '/validate/'+usertoken,
//             success: function(data){
//                if (data==""){
//                  window.location="/"
//                } else{
//                  $('#email').html(data);
//                }
//             },//token is no longer valid (1 hour expiration), they need to log in
//             contentType: "application/text",
//             dataType: 'text' })

//         return tokenEmail;
//     }
// }