const host = `${window.location.protocol}//${window.location.host}`;
async function Auth() {
    console.log("Test")
   username = document.getElementById("username").value
   password = document.getElementById("password").value

    var employee = {
        "EmployeeId": username,
        "FirstName": "Default",
        "LastName": "Default",
        "CompanyEmail": "default@company.com",
        "PersonalEmail": "default@personal.com",
        "PhoneNumber": "0000000000",
        "CountryId": 1,             // Default integer values
        "RegionId": 1,
        "DepartmentId": 1,
        "ManagerId": 1,
        "EmploymentType": "Default",
        "StartDate": "2000-01-01",
        "Salary": 0.0,
        "Benefits": "None",
        "Password": password,
        "EmployeeType": "User",
        "Status": 1
    };


    const response = await FetchRequest("Authenticate", employee);

    
    if (response && response.Token) {
        localStorage.setItem("authToken", response.Token);
        console.log("Token saved:", response.Token);
        window.location.href = 'home.html';
    } else {
        console.log("Authentication failed");
    }
    

}


async function FetchRequest(uri, model) {
    console.log("Req sent");

 
    const token = localStorage.getItem("authToken");


    let headers = {
        'Content-Type': 'application/json'
    };


    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    try {
        const response = await fetch(host + '/' + uri, {
            method: 'POST',
            headers: headers, 
            body: JSON.stringify(model)
        });

        if (response.ok) {
            const data = await response.json(); 
            return data; 
        } else {
            throw new Error('Error performing operation');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation');
        return null; 
    }
}
