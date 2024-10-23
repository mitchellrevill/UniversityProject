// HTML to trigger the javascript,
// Javscript is then going to send a HTTP request to the website (post request)
// Inside the request the endpoint (uri) is going GetAllEmployees (class name of C#)





async function loadEmployees() {
    try {
        const response = await fetch('http://localhost:8000/GetAllEmployees');

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const employees = await response.json();
        displayEmployees(employees);
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
    }
}

function displayEmployees(employees) {
    const employeeList = document.getElementById('employeeList');
    employeeList.innerHTML = ''; // Clear the list

    employees.forEach(employee => {
        const li = document.createElement('li');
        li.textContent = `${employee.FirstName} ${employee.LastName} - ${employee.CompanyEmail}`;
        employeeList.appendChild(li);
    });
}
