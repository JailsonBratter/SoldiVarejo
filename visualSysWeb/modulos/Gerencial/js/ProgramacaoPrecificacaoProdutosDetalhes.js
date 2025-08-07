
function changeFile(f) {
	$fileName = document.getElementById('file-name');
	$fileName.textContent = f.files[0].name;
};