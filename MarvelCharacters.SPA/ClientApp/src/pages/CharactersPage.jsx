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

export default function CharactersPage(props) {
  const [characters, setcharacters] = useState([]);

  useEffect(() => {
    async function loadCharacters() {
      try
      {
        const response = await api.get(`/api/characters?searchString=${props.searchString}`)
        const data = response.data;
        const charsColl = data.map(item => CharacterFactory(item));
        setcharacters(charsColl);
      }
      catch(error)
      {
        console.log(error);
      }
    }
    loadCharacters();
  }, [props.searchString]);

  const classes = useStyles();
  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        {characters.length > 0 && characters.map((tile, index) => (
          <Grid item xs={2} key={index}>
            <CharacterCard character={tile} />
          </Grid>
        ))}
      </Grid>
    </div>
  );
}

