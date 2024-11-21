document.getElementById('claimForm').addEventListener('submit', function (event) {
    event.preventDefault();
    alert('Claim submitted successfully!');
});

document.getElementById('trackingForm').addEventListener('submit', function (event) {
    event.preventDefault();
    alert('Tracking request sent!');
});
//Claim error checking
document.getElementById("submitClaim").addEventListener("click", function(event) {
    const rate = document.getElementById("rate").value;
    const hours = document.getElementById("hours").value;
    
    if (rate < 0 || hours < 0) {
        alert("Rate or Hours cannot be negative.");
        event.preventDefault(); 
    }
});

