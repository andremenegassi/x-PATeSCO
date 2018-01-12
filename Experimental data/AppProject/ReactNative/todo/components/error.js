import React from 'react';
import { Text, StyleSheet } from 'react-native';

const Error = (props) => {
  return props.error ? (
    <Text style={styles.error}>
      {props.error}
    </Text>
  ) :
  null;
}

const styles = StyleSheet.create({
  error: {
    color: 'red',
    fontSize: 12,
    marginLeft: 20,
    marginBottom: 10,
    marginTop: -5,
  }
});

export default Error;
