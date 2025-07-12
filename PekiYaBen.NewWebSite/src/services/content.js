import apiClient from './api'

export class ContentService {
  // Get blog posts
  async getBlogPosts(page = 1, id = null, yazarid = null) {
    try {
      let url = '/Home/Blog'
      const params = {}
      
      if (id) params.id = id
      if (page && page > 1) params.page = page
      if (yazarid) params.yazarid = yazarid
      
      const response = await apiClient.get(url, { params })
      return response.data
    } catch (error) {
      console.error('Error fetching blog posts:', error)
      throw error
    }
  }
  
  // Get single blog post
  async getBlogPost(id, title) {
    try {
      const response = await apiClient.get(`/Home/Blog/${id}/${title}`)
      return response.data
    } catch (error) {
      console.error('Error fetching blog post:', error)
      throw error
    }
  }
  
  // Get FAQ items
  async getFAQs() {
    try {
      const response = await apiClient.get('/Home/FAQ')
      return response.data
    } catch (error) {
      console.error('Error fetching FAQs:', error)
      throw error
    }
  }
  
  // Get products/services
  async getProducts() {
    try {
      const response = await apiClient.get('/Home/GetProducts')
      return response.data
    } catch (error) {
      console.error('Error fetching products:', error)
      throw error
    }
  }
  
  // Get product detail
  async getProductDetail(id, title = '') {
    try {
      let url = '/Account/NewInterview'
      if (id && title) {
        url += `/${id}/${title}`
      }
      const response = await apiClient.get(url)
      return response.data
    } catch (error) {
      console.error('Error fetching product detail:', error)
      throw error
    }
  }
  
  // Submit contact form
  async submitContactForm(formData) {
    try {
      const response = await apiClient.post('/Home/Index', formData)
      return response.data
    } catch (error) {
      console.error('Error submitting contact form:', error)
      throw error
    }
  }
}

export default new ContentService()