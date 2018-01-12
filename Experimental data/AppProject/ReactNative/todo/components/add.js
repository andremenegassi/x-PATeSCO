
import React, { Component } from 'react';
import {
  StyleSheet,
  TextInput,
  View,
  TouchableHighlight,
  Text,
  Platform
} from 'react-native';
import Error from './error';

export default class Add extends Component {
  constructor() {
    super();
    this.state = {
      todo: ''
    };
    this.validateTodo = this.validateTodo.bind(this);
    this.handleAddPress = this.handleAddPress.bind(this);
  }

  validateTodo(todo) {
    let todoError = '';
    if (!todo) {
      todoError = 'Todo is required';
    }
    this.setState({
      todoError
    });
    return todoError;
  }

  handleAddPress() {
    const { todo } = this.state;
    const todoError = this.validateTodo(todo);
    if (!todoError) {
      this.props.onAddTodo({
        todo
      });
    }
  }

  render() {
    return (
      <View style={styles.container}>
        <TextInput
        placeholder="Todo"
        style={styles.input}
        value={this.state.todo}
        blurOnSubmit={true}
        onChangeText={(todo) => this.setState({todo})}
        onBlur={(e) => this.validateTodo(e.nativeEvent.text)}
        />
        <Error error={this.state.todoError} />
        <TouchableHighlight
        style={styles.button}
        onPress={this.handleAddPress}
        >
          <Text style={styles.buttonText}>
            Add Todo
          </Text>
        </TouchableHighlight>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FFFAF6',
    marginTop: (Platform.OS === 'ios') ? 20 : 0,
    marginBottom: 50,
  },
  input: {
    height: 35,
    borderColor: '#ccc',
    borderWidth: (Platform.OS === 'ios') ? 1 : 0,
    borderRadius: 5,
    marginLeft: 20,
    marginRight: 20,
    marginTop: 10,
    marginBottom: 10,
    paddingLeft: 8,
  },
  button: {
    height: 35,
    backgroundColor: '#4FB0A0',
    marginLeft: 20,
    marginRight: 20,
    marginTop: 10,
    marginBottom: 10,
    justifyContent: 'center',
    borderRadius: 5,
  },
  buttonText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: 'white',
    textAlign: 'center',
  }
});

Add.propTypes = {
  onAddTodo: React.PropTypes.func
}
