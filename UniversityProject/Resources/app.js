async function loadEmployees() {
    try {
        const loadEmployeesResponse = await fetch('http://localhost:8000/GetAllEmployees');

        if (!loadEmployeesResponse.ok) {
            throw new Error('Network response was not ok');
        }

        const employees = await loadEmployeesResponse.json();
        console.log('Employees fetched:', employees);  
        displayEmployees(employees);  

    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        alert('Failed to load employee data. Please try again later.'); 
    }
}

function displayEmployees(employees) {

    // Get reference to the tbody of the table
    const tbody = document.querySelector('#jsonTable tbody');

    console.log('Table body element:', tbody); 

    if (!tbody) { 
        console.error('Table body not found');
        return; 
    }

    // Clear any existing rows in the tbody
    tbody.innerHTML = '';

    // Loop through each employee and create a table row
    employees.forEach(employee => {
        const tr = document.createElement('tr');

        //EmployeeId
        const empIdTd = document.createElement('td');
        empIdTd.innerText = employee.EmployeeId;
        tr.appendChild(empIdTd);

        //FirstName
        const firstNameTd = document.createElement('td');
        firstNameTd.innerText = employee.FirstName;
        tr.appendChild(firstNameTd);

        //LastName
        const lastNameTd = document.createElement('td');
        lastNameTd.innerText = employee.LastName;  // Corrected field name
        tr.appendChild(lastNameTd);

        //CompanyEmail
        const companyEmailTd = document.createElement('td');
        companyEmailTd.innerText = employee.CompanyEmail;  // Corrected field name
        tr.appendChild(companyEmailTd);

        //PersonalEmail
        const personalEmailTd = document.createElement('td');
        personalEmailTd.innerText = employee.PersonalEmail;  // Corrected field name
        tr.appendChild(personalEmailTd);

        // Append the row to the tbody
        tbody.appendChild(tr);
    });
}
