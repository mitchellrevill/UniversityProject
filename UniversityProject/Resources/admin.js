function addDepartment() {
  // Get the department name and description from the form
  const departmentName = document.getElementById("departmentName").value;
  const departmentDescription = document.getElementById("departmentDescription").value;

  // Send a request to the backend API to add the department
  fetch('/add-department', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      name: departmentName,
      description: departmentDescription
    })
  })
  .then(response => response.json())
  .then(data => {
    // Handle the response from the server
    if (data.success) {
      // Department added successfully, update the table or display a message
      console.log('Department added successfully');
    } else {
      // Handle errors
      console.error('Error adding department:', data.error);
    }
  })
  .catch(error => {
    console.error('Error:', error);
  });
}