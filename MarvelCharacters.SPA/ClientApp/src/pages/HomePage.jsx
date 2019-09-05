import React, { useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import { CharacterFactory } from '../factories/characterFactory';
import CharacterCard from '../components/CharacterCard';
import api from '../services/api';

const useStyles = makeStyles(theme => ({
  root: {
    flexGrow: 1,
  },
  icon: {
    color: 'rgba(255, 255, 255, 0.54)',
  },
}));

export default function HomePage() {
  const [characters, setcharacters] = useState([]);

  useEffect(() => {
    async function loadCharacters() {
      const response = await api.get('/api/characters?searchString=Spider')
      const data = response.data;
      setcharacters(data.map(item => CharacterFactory(item)))
    }
    loadCharacters();
  })

  const classes = useStyles();
  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        {characters.length > 0 && characters.map((tile, index) => (
          <Grid item xs={2}>
            <CharacterCard character={tile} />
          </Grid>
        ))}
      </Grid>
    </div>
  );
}