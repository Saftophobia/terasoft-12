
function show(x) {
	document.getElementById("photo").innerHTML = document.getElementById(x).innerHTML;
	document.getElementById("photo").className = "livphoto";
}

function hide() {
	document.getElementById("photo").innerHTML = document.getElementById("photohidden").innerHTML;
	document.getElementById("photo").className = "photo";
}