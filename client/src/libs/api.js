const BASE_URL = 'http://localhost:5000'

export const apiGet =  async (url) => {
    try {
        const response = await fetch(BASE_URL + url, { 
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
            },
        });
        if (response.status != 200) {
            const text = await response.text();
            throw new Error(text);
        }
        const data = response.json();
        return data;
    } catch (error) {
        console.error('Error in API call (GET)', url, 'fetching data: ', error);
    }
} 

export const apiPost = async (url, body={}) => {
    try {
        const response = await fetch(BASE_URL + url, { 
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body)
        });
        if (response.status != 200) {
            const text = await response.text();
            throw new Error(text);
        }
        const data = response.json();
        return data;
    } catch (error) {
        console.error('Error in API call (POST)', url, 'fetching data: ', error);
    }
}

export const apiPut = async (url, body={}) => {
    try {
        const response = await fetch(BASE_URL + url, { 
            method: "PUT",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body)
        });
        if (response.status != 200) {
            const text = await response.text();
            throw new Error(text);
        }
        const data = response.json();
        return data;
    } catch (error) {
        console.error('Error in API call (POST)', url, 'fetching data: ', error);
    }
}

export const apiDelete = async (url) => {
    try {
        const response = await fetch(BASE_URL + url, { 
            method: "DELETE",
        });
        if (response.status != 200) {
            const text = await response.text();
            throw new Error(text);
        }
    } catch (error) {
        console.error('Error in API call (DELETE)', url, 'fetching data: ', error);
    }
}