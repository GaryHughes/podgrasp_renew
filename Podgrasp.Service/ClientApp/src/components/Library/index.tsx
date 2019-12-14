import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService'

interface Props {}

export class Library extends Component {
  
    constructor(props: Props) {
    
        super(props);
        
        this.state = {
          podcasts: null,
          isLoading: false,
        };
    
        this.fetchPodcasts = this.fetchPodcasts.bind(this);
        this.setPodcasts = this.setPodcasts.bind(this);
    }

    componentDidMount() {
        this.fetchPodcasts();
    }

    async fetchPodcasts() {
        console.log("fetching podcasts");
        this.setState({ isLoading: true });
        const token = await authService.getAccessToken();
        fetch('Api/1.0/Podcasts', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` } 
        })
        .then(result => this.setPodcasts(result))
        .catch(error => this.setState({ error }));
    }
    
    setPodcasts(result: any) {
        this.setState({
            podcats: result,
            isLoading: false
        });
    }

    render() {
        return (
        <div>
            Library
        </div>
        );
    }

}
