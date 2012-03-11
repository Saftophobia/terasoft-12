
function show(x) {
	document.getElementById("gallery").innerHTML = document.getElementById(x).innerHTML;
}

function hide() {
	document.getElementById("gallery").innerHTML = "<img src='glasses.png' />";
}