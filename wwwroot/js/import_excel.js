function import_excel(complete_function) {
    var input = document.createElement('input');
    input.setAttribute('type', 'file');
    input.onchange = function (event) {
        var files = event.target.files;
        if(files.length==0){
            alert("Please choose any file...");
            return;
        }
        var filename = files[0].name;
        var extension = filename.substring(filename.lastIndexOf(".")).toUpperCase();
        if (extension == '.XLS' || extension == '.XLSX') {
            excelFileToJSON(files[0], complete_function);
        }else{
            alert("Please select a valid excel file.");
        }
    }
    input.click();
}
//Method to read excel file and convert it into JSON 
function excelFileToJSON(file, complete_function){
    try {
        var reader = new FileReader();
        reader.readAsBinaryString(file);
        reader.onload = function(e) {
            var data = e.target.result;
            var workbook = XLSX.read(data, {
                type : 'binary'
            });
            var result = {};
            workbook.SheetNames.forEach(function(sheetName) {
            var roa = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
            if (roa.length > 0) {
                result[sheetName] = roa;
            }
            });
            //displaying the json result
            complete_function(JSON.stringify(result, null, 4));  
        }
    }catch(e){
        alert(e);
    }
}